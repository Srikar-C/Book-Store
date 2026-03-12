using CatalogueService.Repositories;
using Confluent.Kafka;
using MongoDB.Driver;
using System.Text.Json;

public class KafkaConsumerService : BackgroundService
{
    private readonly IConfiguration _config;
    private readonly MongoRepo _repo;
    public KafkaConsumerService(IConfiguration config, MongoRepo repo)
    {
        _config = config;
        _repo = repo;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var consumerConfig = new ConsumerConfig
        {
            BootstrapServers = "localhost:9092",
            GroupId = "catalogue-group",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        var consumer = new ConsumerBuilder<Ignore, string>(consumerConfig).Build();

        consumer.Subscribe("order-created");

        return Task.Run(async () =>
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var result = consumer.Consume(stoppingToken);
                Console.WriteLine("Kafka Event Received:"+result);

                var orderEvent = JsonSerializer.Deserialize<OrderCreatedEvent>(result.Message.Value);

                foreach (var book in orderEvent.Books)
                {
                    Console.WriteLine($"Reduce stock for BookId: {book.Id}, {book.Quantity}, {book.Count}, {book.SoldOut}");
                    var filter = Builders<BookModel>.Filter.Eq(b=>b.Id,book.Id);
                    var update = Builders<BookModel>.Update.Inc(c=>c.Quantity, -book.Count).Inc(c=>c.SoldOut, book.Count);
                    await _repo.UpdateQuantityAsync("Books",filter,update);
                }
            }
        }, stoppingToken);
    }
}