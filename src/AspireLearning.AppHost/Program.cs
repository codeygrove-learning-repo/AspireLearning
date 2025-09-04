using Microsoft.Extensions.Configuration;

var builder = DistributedApplication.CreateBuilder(args);

var keyvault = builder
    .AddAzureKeyVault("keyvault")
    .RunAsExisting("codeygrove-keyvault", "codeygrove-rg");

var mongoConnectionString = keyvault.GetSecret("codeygrove-mongodb-connstr");
var orderServiceBusConnectionString = keyvault.GetSecret("codeygrove-servicebus-connstr");
var eventHubConnectionString = keyvault.GetSecret("codeygrove-eventhub-connstr");

// Load configuration
var config = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

var logLevel = config["Logging:LogLevel:Default"] ?? "Information";

var orderApi = builder.AddProject<Projects.AspireLearning_OrderApi>("aspirelearning-orderapi")
    .WithEnvironment("MongoDb__ConnectionString", mongoConnectionString)
    .WithEnvironment("ServiceBus__ConnectionString", orderServiceBusConnectionString)
    .WithEnvironment("Logging__LogLevel__Default", logLevel)
    .WaitFor(keyvault); // wait for keyvault resource to be ready

var catalogApi = builder.AddProject<Projects.AspireLearning_CatalogApi>("aspirelearning-catalogapi")
    .WithEnvironment("MongoDb__ConnectionString", mongoConnectionString)
    .WithEnvironment("Logging__LogLevel__Default", logLevel)
    .WaitFor(keyvault); // wait for keyvault resource to be ready

// OrderProcessor
var orderProcessor = builder.AddProject<Projects.AspireLearning_OrderProcessor>("aspirelearning-orderprocessor")
    .WithEnvironment("ServiceBus__ConnectionString", orderServiceBusConnectionString)
    .WithEnvironment("EventHub__ConnectionString", eventHubConnectionString)
    .WithEnvironment("Logging__LogLevel__Default", logLevel)
    .WaitFor(keyvault); // wait for keyvault resource to be ready

// InvoiceConsumer
var invoiceConsumer = builder.AddProject<Projects.AspireLearning_InvoiceConsumer>("aspirelearning-invoiceconsumer")
    .WithEnvironment("EventHub__ConnectionString", eventHubConnectionString)
    .WithEnvironment("Logging__LogLevel__Default", logLevel)
    .WaitFor(keyvault); // wait for keyvault resource to be ready

// VendorResupplyConsumer
var resupplyConsumer = builder.AddProject<Projects.AspireLearning_VendorResupplyConsumer>("aspirelearning-vendorresupplyconsumer")
    .WithEnvironment("EventHub__ConnectionString", eventHubConnectionString)
    .WithEnvironment("Logging__LogLevel__Default", logLevel)
    .WaitFor(keyvault); // wait for keyvault resource to be ready

// ReplenishmentConsumer
var replenishmentConsumer = builder.AddProject<Projects.AspireLearning_ReplenishmentConsumer>("aspirelearning-replenishmentconsumer")
    .WithEnvironment("EventHub__ConnectionString", eventHubConnectionString)
    .WithEnvironment("Logging__LogLevel__Default", logLevel)
    .WaitFor(keyvault); // wait for keyvault resource to be ready

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
    .WaitFor(replenishmentConsumer)
    .WaitFor(keyvault); // wait for keyvault resource to be ready

builder.AddViteApp("mobile", "../mobileapp")
    .WaitFor(orderApi)
    .WaitFor(keyvault) // wait for keyvault resource to be ready
    .WithEnvironment(ctx =>
    {
        //https://github.com/dotnet/aspire/discussions/5954#discussioncomment-10840035
        ctx.EnvironmentVariables.Add("VITE_ORDER_API_URL", orderApi.GetEndpoint("https").Url);
    })
    .WithNpmPackageInstallation();

builder.Build().Run();
