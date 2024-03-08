using System.Text.Json;
using Confluent.Kafka;
using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;
using JsonConsole;
using MicroserviceFirst.API.Products.ProductStream.Events;
using Microsoft.Extensions.Configuration;

namespace MicroserviceFirst.API.Products.ProductStream
{
    public class KafkaProductStream(IConfiguration configuration)
    {
        public const string ProductStreamTopic = "product-stream-topic";
        public const string ProductStreamGroupName = "product-stream-group-a";

        //public KafkaProductStream(IConfiguration configuration)
        //{
        //    var config = new ProducerConfig
        //        { BootstrapServers = configuration.GetSection("Kafka")["BootstrapServers"], Acks = Acks.All };

        //    var schemaRegistryConfig = new SchemaRegistryConfig
        //    {
        //        Url = "http://localhost:8081"
        //    };

        //    var schemaRegistry = new CachedSchemaRegistryClient(schemaRegistryConfig);


        //    //_producer = new ProducerBuilder<string, ProductCreatedEvent>(config).SetValueSerializer(
        //    //    new JsonSerializer<ProductCreatedEvent>(schemaRegistry));


        //}

        public async Task Publish(string key, ProductCreatedEvent productCreatedEvent)
        {
            //if (@event is ProductCreatedEvent productCreatedEvent)
            //{
            var producer = new JsonProducer<ProductCreatedEvent>(
                configuration.GetSection("Kafka")["BootstrapServers"]!, "http://localhost:8081",
                ProductStreamTopic);

            producer.Build();


            await producer.ProduceAsync(productCreatedEvent);
            //}
        }
    }
}