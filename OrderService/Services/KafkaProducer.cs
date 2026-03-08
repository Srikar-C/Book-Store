using Confluent.Kafka;
using System.Text.Json;

public class KafkaProducer
{
    private readonly string _bootstrapServers = "localhost:9092";

    public async Task ProduceAsync(string topic, object message)
    {
        var config = new ProducerConfig
        {
            BootstrapServers = _bootstrapServers
        };

        using var producer = new ProducerBuilder<Null, string>(config).Build();

        var json = JsonSerializer.Serialize(message);

        await producer.ProduceAsync(topic, new Message<Null, string>
        {
            Value = json
        });
    }
}