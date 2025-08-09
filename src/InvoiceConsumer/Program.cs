var builder = Host.CreateApplicationBuilder(args);
builder.AddServiceDefaults();

builder.Services.AddHostedService<AspireLearning.InvoiceConsumer.Worker>();

var host = builder.Build();
host.Run();