using MicroserviceFirst.API.KafkaServiceBus.Producer;

namespace MicroserviceFirst.API.Products.ProductStream;

public class KafkaProductStream(IConfiguration configuration)
{
    public const string ProductStreamTopic = "product-stream-topic2";
    public const string ProductStreamGroupName = "product-stream-group-a";


    public async Task Publish<T>(string key, T @event) where T : class
    {
        var producer = new AvroProducer<T>(
            configuration.GetSection("Kafka")["BootstrapServers"]!, "http://localhost:8081",
            ProductStreamTopic);

        producer.Build();


        await producer.ProduceAsync(@event, key);
    }
}