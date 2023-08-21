using EventBus.Core;
using Microsoft.Extensions.Logging;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System.Net.Sockets;

namespace EventBus.RabbitMq
{
    public class RabbitMQConnection : IRabbitMQPersistentConnection
    {
        private readonly object _lock = new object();

        private readonly IConnectionFactory _connectionFactory;
        private readonly ILogger<RabbitMQConnection> _logger;

        private bool Disposed = false;
        private IConnection? _connection;

        public event EventBusConnectionEventHandler? OnConnected;

        public RabbitMQConnection(
            IConnectionFactory connectionFactory,
            ILogger<RabbitMQConnection> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }


        public bool IsConnected => _connection is not null && _connection.IsOpen && !Disposed;


        public IModel CreateModel()
        {
            if (!IsConnected)
            {
                throw new InvalidOperationException("RabbitMQ is not connected.");
            }
            return _connection!.CreateModel();
        }

        public bool TryConnect()
        {
            if (IsConnected) return true;

            lock (_lock)
            {
                var policy = Polly.Policy
                    .Handle<SocketException>()
                    .Or<ConnectFailureException>()
                    .WaitAndRetry(6, x => TimeSpan.FromSeconds(Math.Pow(2, x)), (ex, time) =>
                    {
                        _logger.LogError(ex, "RabbitMQ connection failed after {TimeOut}s", $"{time.TotalSeconds:n1}");
                    });

                policy.Execute(() =>
                {
                    _connection = _connectionFactory.CreateConnection();
                });

                if (IsConnected)
                {
                    _logger.LogInformation("RabbitMQ Client Successfully connected");

                    _connection!.ConnectionShutdown += OnConnectionShutdown;
                    _connection!.ConnectionBlocked += OnConnectionBlocked;
                    _connection!.CallbackException += OnCallbackException;

                    OnConnected?.Invoke(this, new EventBusConnectionEventArgs());

                    return true;
                }
                else
                {
                    _logger.LogCritical("RabbitMQ Client Could not connect");
                    return false;
                }
            }
        }


        private void OnConnectionBlocked(object? sender, ConnectionBlockedEventArgs e)
        {
            if (Disposed) return;

            _logger.LogWarning("A RabbitMQ connection is shutdown. Trying to re-connect...");

            TryConnect();
        }

        void OnCallbackException(object? sender, CallbackExceptionEventArgs e)
        {
            if (Disposed) return;

            _logger.LogWarning("A RabbitMQ connection throw exception. Trying to re-connect...");

            TryConnect();
        }

        void OnConnectionShutdown(object? sender, ShutdownEventArgs reason)
        {
            if (Disposed) return;

            _logger.LogWarning("A RabbitMQ connection is on shutdown. Trying to re-connect...");

            TryConnect();
        }

        public void Dispose()
        {
            if (Disposed) return;

            Disposed = true;

            try
            {
                var connection = _connection;
                if (connection is not null)
                {
                    connection.ConnectionShutdown -= OnConnectionShutdown;
                    connection.CallbackException -= OnCallbackException;
                    connection.ConnectionBlocked -= OnConnectionBlocked;
                    connection.Dispose();
                }
            }
            catch (IOException ex)
            {
                _logger.LogCritical(ex.ToString());
            }
        }

    }
}
