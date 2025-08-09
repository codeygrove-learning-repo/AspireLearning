using Azure.Messaging.EventHubs.Consumer;

namespace AspireLearning.VendorResupplyConsumer;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IConfiguration _config;

    public Worker(ILogger<Worker> logger, IConfiguration config)
    {
        _logger = logger;
        _config = config;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var eventHubConnectionString = _config["EventHub:ConnectionString"];
        var eventHubName = _config["EventHub:EventHubName"];
        var consumerGroup = _config["EventHub:ConsumerGroup"];

        await using var consumer = new EventHubConsumerClient(consumerGroup, eventHubConnectionString, eventHubName);

        Console.WriteLine("VendorResupplyConsumer started. Listening for events...");

        while (!stoppingToken.IsCancellationRequested)
        {
            await foreach (PartitionEvent partitionEvent in consumer.ReadEventsAsync(CancellationToken.None))
            {
                if (partitionEvent.Data?.EventBody != null)
                {
                    string data = partitionEvent.Data.EventBody.ToString();
                    Console.WriteLine($"[VendorResupplyConsumer] Received event: {data}");
                }
            }
        }
    }
}
