var builder = Host.CreateApplicationBuilder(args);
builder.AddServiceDefaults();

builder.Services.AddHostedService<AspireLearning.VendorResupplyConsumer.Worker>();

var host = builder.Build();
host.Run();