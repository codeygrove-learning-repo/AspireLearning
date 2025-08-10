using Microsoft.Extensions.Configuration;

var builder = DistributedApplication.CreateBuilder(args);

// Load configuration
var config = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

var mongoConnectionString = config["MongoDb:ConnectionString"];
var mongoDatabaseName = config["MongoDb:DatabaseName"];
var mongoCatalogCollection = config["MongoDb:CatalogCollectionName"];
var mongoOrderCollection = config["MongoDb:OrderCollectionName"];
var logLevel = config["Logging:LogLevel:Default"] ?? "Information";

//var keyvault = builder
//    .AddAzureKeyVault("keyvault")
//    .RunAsExisting("codeygrove-keyvault", "codeygrove-rg");

var catalogApi = builder.AddProject<Projects.AspireLearning_CatalogApi>("aspirelearning-catalogapi")
    //.WithEnvironment("MongoDb__ConnectionString", keyvault.GetSecret("codeygrove-mongodb-connstr")) //Get Secret
    .WithEnvironment("MongoDb__ConnectionString", mongoConnectionString)
    .WithEnvironment("MongoDb__DatabaseName", mongoDatabaseName)
    .WithEnvironment("MongoDb__CatalogCollectionName", mongoCatalogCollection)
    .WithEnvironment("MongoDb__OrderCollectionName", mongoOrderCollection)
    .WithEnvironment("Logging__LogLevel__Default", logLevel)
    ;
    //.WaitFor(keyvault);

var orderServiceBusConnectionString = config["ServiceBus:ConnectionString"];

var orderApi = builder.AddProject<Projects.AspireLearning_OrderApi>("aspirelearning-orderapi")
    //.WithEnvironment("MongoDb__ConnectionString", keyvault.GetSecret("codeygrove-mongodb-connstr")) //Get Secret
    //.WithEnvironment("ServiceBus__ConnectionString", keyvault.GetSecret("codeygrove-servicebus-connstr"))
    .WithEnvironment("MongoDb__ConnectionString", mongoConnectionString)
    .WithEnvironment("MongoDb__DatabaseName", mongoDatabaseName)
    .WithEnvironment("MongoDb__CatalogCollectionName", mongoCatalogCollection)
    .WithEnvironment("MongoDb__OrderCollectionName", mongoOrderCollection)
    .WithEnvironment("ServiceBus__ConnectionString", orderServiceBusConnectionString)
    .WithEnvironment("Logging__LogLevel__Default", logLevel)
    ;
    //.WaitFor(keyvault);

var eventHubConnectionString = config["EventHub:ConnectionString"];

// OrderProcessor
var orderProcessor = builder.AddProject<Projects.AspireLearning_OrderProcessor>("aspirelearning-orderprocessor")
    //.WithEnvironment("ServiceBus__ConnectionString", keyvault.GetSecret("codeygrove-servicebus-connstr"))
    //.WithEnvironment("EventHub__ConnectionString", keyvault.GetSecret("codeygrove-eventhub-connstr"))
    .WithEnvironment("ServiceBus__ConnectionString", orderServiceBusConnectionString)
    .WithEnvironment("EventHub__ConnectionString", eventHubConnectionString)
    .WithEnvironment("Logging__LogLevel__Default", logLevel)
    ;
    //.WaitFor(keyvault);

// InvoiceConsumer
var invoiceConsumer = builder.AddProject<Projects.AspireLearning_InvoiceConsumer>("aspirelearning-invoiceconsumer")
    //.WithEnvironment("EventHub__ConnectionString", keyvault.GetSecret("codeygrove-eventhub-connstr"))
    .WithEnvironment("EventHub__ConnectionString", eventHubConnectionString)
    .WithEnvironment("Logging__LogLevel__Default", logLevel);

// VendorResupplyConsumer
var resupplyConsumer = builder.AddProject<Projects.AspireLearning_VendorResupplyConsumer>("aspirelearning-vendorresupplyconsumer")
    //.WithEnvironment("EventHub__ConnectionString", keyvault.GetSecret("codeygrove-eventhub-connstr"))
    .WithEnvironment("EventHub__ConnectionString", eventHubConnectionString)
    .WithEnvironment("Logging__LogLevel__Default", logLevel)
    ;
//.WaitFor(keyvault);

// ReplenishmentConsumer
var replenishmentConsumer = builder.AddProject<Projects.AspireLearning_ReplenishmentConsumer>("aspirelearning-replenishmentconsumer")
    //.WithEnvironment("EventHub__ConnectionString", keyvault.GetSecret("codeygrove-eventhub-connstr"))
    .WithEnvironment("EventHub__ConnectionString", eventHubConnectionString)
    .WithEnvironment("Logging__LogLevel__Default", logLevel)
    ;
    //.WaitFor(keyvault);

// Only start web portal after all services are ready
builder.AddProject<Projects.AspireLearning_WebPortal>("aspirelearning-webportal")
    .WithEnvironment("Logging__LogLevel__Default", logLevel)
    .WithReference(orderApi)
    .WithReference(catalogApi)
    .WaitFor(orderProcessor)
    .WaitFor(invoiceConsumer)
    .WaitFor(resupplyConsumer)
    .WaitFor(replenishmentConsumer);

builder.AddViteApp("mobile", "../mobileapp")
    .WithReference(orderApi)
    .WaitFor(orderApi)
    .WithEnvironment(ctx =>
    {
        ctx.EnvironmentVariables.Add("VITE_ORDER_API_URL", orderApi.GetEndpoint("https").Url);
    })
    .WithExternalHttpEndpoints()
    .WithNpmPackageInstallation()
    .PublishAsDockerFile();

builder.Build().Run();