using Avro.Generic;
using MicroserviceFirst.API.KafkaServiceBus.Consumer;
using MicroserviceFirst.API.Models;
using MicroserviceFirst.API.Products.ProductStream;

namespace MicroserviceFirst.API.BackgroundServices;

public class ProductStreamBackgroundServices(IConfiguration configuration, IServiceProvider serviceProvider)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var vehicleConsumer = new AvroConsumer<GenericRecord>(
            configuration.GetSection("Kafka")["BootstrapServers"]!, "http://localhost:8081",
            KafkaProductStream.ProductStreamGroupName, KafkaProductStream.ProductStreamTopic);


        vehicleConsumer.Build();


        while (!stoppingToken.IsCancellationRequested)
        {
            //var consumeResult = vehicleConsumer.Consume(stoppingToken);

            var consumeResult = vehicleConsumer.Consumer!.Consume(1000);


            if (consumeResult is not null)
            {
                var valueResult = consumeResult.Message.Value;


                using var scope = serviceProvider.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                if (valueResult.Schema.Name == "ProductCreatedEvent")
                {
                    //read field
                    var id = Guid.Parse(valueResult["Id"].ToString()!);
                    var name = Convert.ToString(valueResult["Name"]);
                    var price = Convert.ToDouble(valueResult["Price"]);
                    var stock = Convert.ToInt32(valueResult["Stock"]);
                    var categoryId = Convert.ToInt32(valueResult["CategoryId"]);


                    var newProduct = new Product
                    {
                        Id = id,
                        Name = name!,
                        Price = price,
                        Stock = stock,
                        CategoryId = categoryId
                    };


                    dbContext.Products.Add(newProduct);
                    await dbContext.SaveChangesAsync(stoppingToken);
                }
                else if (valueResult.Schema.Name == "ProductNameUpdatedEvent")
                {
                    //read field
                    var id = Guid.Parse(valueResult["Id"].ToString()!);
                    var name = Convert.ToString(valueResult["Name"]);

                    var product = await dbContext.Products.FindAsync(id);
                    if (product is not null)
                    {
                        product.Name = name!;
                        await dbContext.SaveChangesAsync(stoppingToken);
                    }
                }

                Console.WriteLine(valueResult.Schema.Name);
                vehicleConsumer.Consumer.Commit(consumeResult);
            }


            await Task.Delay(1000, stoppingToken);
        }
    }
}