using Common;
using Orders.SignalR.Events;
using Orders.SignalR.Hubs;

await Extensions.RunInLoggerAsync(async () =>
{
    var builder = WebApplication.CreateBuilder(args);

    builder.AddServiceDefaults("OrdersSignalR");
    builder.Services.AddSignalR();
    builder.AddEventHandlers(typeof(OrderStatusChangedToSubmittedEvent).Assembly);

    var app = builder.Build();

    app.UseDefaultPipeline();

    app.MapHub<OrderingHub>("/Hubs/Orders/Updates");

    app.ConfigureEventBus(new Common.Options.UseEventBusOptions()
        .Subscribe<OrderStatusChangedToSubmittedEvent, OrderStatusChangedToSubmittedEventHandler>()
        .Subscribe<OrderMarkedAsPaidEvent, OrderMarkedAsPaidEventHandler>());

    await app.RunAsync();
}, "Orders.SignalR");