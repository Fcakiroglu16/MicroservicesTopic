using System.Reflection;
using MediatR;
using MicroserviceFirst.API.BackgroundServices;
using MicroserviceFirst.API.KafkaServiceBus;
using MicroserviceFirst.API.Models;
using MicroserviceFirst.API.Products.ProductCreate;
using MicroserviceFirst.API.Products.ProductNameUpdate;
using MicroserviceFirst.API.Products.ProductStream;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
builder.Services.AddScoped<KafkaProductStream>();
builder.Services.AddSingleton<KafkaServiceBusInitialize>();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));

builder.Services.AddHostedService<ProductStreamBackgroundServices>();

//builder.Services.AddHostedService<ProductNameUpdatedBackgroundServices>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var kafkaServiceBusInitialize = scope.ServiceProvider.GetRequiredService<KafkaServiceBusInitialize>();
    await kafkaServiceBusInitialize.CreateTopics();


    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    if (!dbContext.Categories.Any())
    {
        dbContext.Categories.Add(new Category { Name = "Category 1" });
        dbContext.Categories.Add(new Category { Name = "Category 2" });
        dbContext.Categories.Add(new Category { Name = "Category 3" });


        dbContext.SaveChanges();
    }
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapPost("/api/products/create", async (ProductCreateCommand productCreateCommand, IMediator mediator) =>
{
    var result = await mediator.Send(productCreateCommand);

    return Results.Created(string.Empty, result);
});

app.MapPost("/api/products/UpdateProductName",
    async (UpdateProductNameCommand updateProductNameCommand, IMediator mediator) =>
    {
        var result = await mediator.Send(updateProductNameCommand);

        return Results.NoContent();
    });


app.Run();