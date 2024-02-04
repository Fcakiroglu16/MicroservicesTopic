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


builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapPost("/api/products/create", async (AppDbContext context) =>
{
    context.Products.Add(new Product() { Name = "Pen 1", Price = 100 });
    context.SaveChanges();

    return Results.Ok("Product created");
});

app.MapPost("/api/products/update", async (AppDbContext context) =>
{
    var product = context.Products.First();
    product.Name = $"{product.Name}- {product.Name}";
    context.SaveChanges();

    return Results.Ok("Product created");
});

app.MapGet("/api/SendRequestToMicroserviceTwo",
    async (MicroserviceSecondService secondMicroserviceService) =>
    {
        var response = await secondMicroserviceService.GetProducts();


        return Results.Ok(response);
    }).WithName("SendRequestToMicroserviceTwo").WithOpenApi();

app.Run();