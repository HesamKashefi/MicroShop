using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace Orders.SignalR.Hubs
{
    [Authorize]
    public class OrderingHub : Hub
    {
        private readonly ILogger<OrderingHub> _logger;

        public OrderingHub(ILogger<OrderingHub> logger)
        {
            _logger = logger;
        }

        public override async Task OnConnectedAsync()
        {
            var username = Context.GetHttpContext()!.User.FindFirstValue("name")!;
            _logger.LogTrace("User {UserName} Connected To OrderingHub", username);
            await this.Groups.AddToGroupAsync(this.Context.ConnectionId, username, this.Context.ConnectionAborted);
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var username = Context.GetHttpContext()!.User.FindFirstValue("name")!;
            _logger.LogTrace("User {UserName} Disconnected From OrderingHub", username);
            await this.Groups.RemoveFromGroupAsync(this.Context.ConnectionId, username, this.Context.ConnectionAborted);
        }
    }
}
