var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddHostedService<AspireLearning.ReplenishmentConsumer.Worker>();

var host = builder.Build();
host.Run();