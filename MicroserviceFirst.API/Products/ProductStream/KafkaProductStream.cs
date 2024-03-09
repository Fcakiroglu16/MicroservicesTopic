using MicroserviceFirst.API.KafkaServiceBus.Producer;
using MicroserviceFirst.API.Products.ProductStream.Events;
using MicroserviceFirst.API.ServiceBus;

namespace MicroserviceFirst.API.Products.ProductStream
{
    public class KafkaProductStream(IConfiguration configuration)
    {
        public const string ProductStreamTopic = "product-stream-topic";
        public const string ProductStreamGroupName = "product-stream-group-a";


        public async Task Publish(string key, ProductCreatedEvent productCreatedEvent)
        {
            var producer = new JsonProducer<ProductCreatedEvent>(
                configuration.GetSection("Kafka")["BootstrapServers"]!, "http://localhost:8081",
                ProductStreamTopic);

            producer.Build();


            await producer.ProduceAsync(productCreatedEvent, key);
            //}
        }
    }
}