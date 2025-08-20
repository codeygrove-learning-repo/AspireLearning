using Microsoft.Extensions.Configuration;

var builder = DistributedApplication.CreateBuilder(args);

// Load configuration
var config = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

var mongoConnectionString = config["MongoDb:ConnectionString"];
var orderServiceBusConnectionString = config["ServiceBus:ConnectionString"];
var logLevel = config["Logging:LogLevel:Default"] ?? "Information";

var orderApi = builder.AddProject<Projects.AspireLearning_OrderApi>("aspirelearning-orderapi")
    .WithEnvironment("MongoDb__ConnectionString", mongoConnectionString)
    .WithEnvironment("ServiceBus__ConnectionString", orderServiceBusConnectionString)
    .WithEnvironment("Logging__LogLevel__Default", logLevel);

var catalogApi = builder.AddProject<Projects.AspireLearning_CatalogApi>("aspirelearning-catalogapi")
    .WithEnvironment("MongoDb__ConnectionString", mongoConnectionString)
    .WithEnvironment("Logging__LogLevel__Default", logLevel);

// This setting is required by all consumer
var eventHubConnectionString = config["EventHub:ConnectionString"];

// OrderProcessor
var orderProcessor = builder.AddProject<Projects.AspireLearning_OrderProcessor>("aspirelearning-orderprocessor")
    .WithEnvironment("ServiceBus__ConnectionString", orderServiceBusConnectionString)
    .WithEnvironment("EventHub__ConnectionString", eventHubConnectionString)
    .WithEnvironment("Logging__LogLevel__Default", logLevel);

// InvoiceConsumer
var invoiceConsumer = builder.AddProject<Projects.AspireLearning_InvoiceConsumer>("aspirelearning-invoiceconsumer")
    .WithEnvironment("EventHub__ConnectionString", eventHubConnectionString)
    .WithEnvironment("Logging__LogLevel__Default", logLevel);

// VendorResupplyConsumer
var resupplyConsumer = builder.AddProject<Projects.AspireLearning_VendorResupplyConsumer>("aspirelearning-vendorresupplyconsumer")
    .WithEnvironment("EventHub__ConnectionString", eventHubConnectionString)
    .WithEnvironment("Logging__LogLevel__Default", logLevel);

// ReplenishmentConsumer
var replenishmentConsumer = builder.AddProject<Projects.AspireLearning_ReplenishmentConsumer>("aspirelearning-replenishmentconsumer")
    .WithEnvironment("EventHub__ConnectionString", eventHubConnectionString)
    .WithEnvironment("Logging__LogLevel__Default", logLevel);

// Only start web portal after all services are ready
builder.AddProject<Projects.AspireLearning_WebPortal>("aspirelearning-webportal")
    .WithEnvironment("Logging__LogLevel__Default", logLevel)
    .WithReference(orderApi)
    .WithReference(catalogApi)
    .WaitFor(orderApi)
    .WaitFor(catalogApi)
    .WaitFor(orderProcessor)
    .WaitFor(invoiceConsumer)
    .WaitFor(resupplyConsumer)
    .WaitFor(replenishmentConsumer);

builder.AddViteApp("mobile", "../mobileapp")
    .WaitFor(orderApi)
    .WithEnvironment(ctx =>
    {
        //https://github.com/dotnet/aspire/discussions/5954#discussioncomment-10840035
        ctx.EnvironmentVariables.Add("VITE_ORDER_API_URL", orderApi.GetEndpoint("https").Url);
    })
    .WithNpmPackageInstallation();

builder.Build().Run();
