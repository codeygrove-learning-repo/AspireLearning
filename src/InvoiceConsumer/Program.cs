using Azure.Messaging.EventHubs.Consumer;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AspireLearning.InvoiceConsumer
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var eventHubConnectionString = config["EventHub:ConnectionString"];
            var eventHubName = config["EventHub:EventHubName"];
            var consumerGroup = config["EventHub:ConsumerGroup"];

            await using var consumer = new EventHubConsumerClient(consumerGroup, eventHubConnectionString, eventHubName);
            Console.WriteLine("InvoiceConsumer started. Listening for events...");
            await foreach (PartitionEvent partitionEvent in consumer.ReadEventsAsync(CancellationToken.None))
            {
                if (partitionEvent.Data?.EventBody != null)
                {
                    string data = partitionEvent.Data.EventBody.ToString();
                    Console.WriteLine($"[InvoiceConsumer] Received event: {data}");
                }
            }
        }
    }
}
