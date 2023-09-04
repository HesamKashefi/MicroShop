using Common;
using Orders.SignalR.Events;
using Orders.SignalR.Hubs;

await Extensions.RunInLoggerAsync(async () =>
{
    var builder = WebApplication.CreateBuilder(args);

    builder.AddServiceDefaults();
    builder.Services.AddSignalR();

    var app = builder.Build();

    app.UseDefaultPipeline();

    app.MapHub<OrderingHub>("Hubs/Orders/Update");

    app.ConfigureEventBus(new Common.Options.UseEventBusOptions()
        .Subscribe<OrderStatusChangedToSubmittedEvent, OrderStatusChangedToSubmittedEventHandler>());

    await app.RunAsync();
}, "Orders.SignalR");