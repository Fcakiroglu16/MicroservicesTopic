using MicroserviceFirst.API;
using MicroserviceFirst.API.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient<MicroserviceSecondService>(configure =>
    configure.BaseAddress = new Uri(builder.Configuration.GetSection("MicroserviceBaseUrls")["MicroserviceSecond"]!));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapGet("/api/products/{productId}", (int productId) =>
{
    var productList = new List<Product>
    {
        new Product { Id = 1, Name = "Product 1", Price = 100 },
        new Product { Id = 2, Name = "Product 2", Price = 200 },
        new Product { Id = 3, Name = "Product 3", Price = 300 },
        new Product { Id = 4, Name = "Product 4", Price = 400 },
        new Product { Id = 5, Name = "Product 5", Price = 500 },
    };


    return Results.Ok(productList.FirstOrDefault(x => x.Id == productId));
});


app.Run();