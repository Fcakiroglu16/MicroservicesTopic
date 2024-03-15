using MicroserviceSecond.API.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapGet("api/stocks/{productId}", (int productId) =>
{
    var stock = new Stock()
    {
        Id = 20,
        ProductId = 1,
        Code = "ABC",
        Count = 20,
        Type = "A"
    };

    return Results.Ok(stock);
});

#region legacy routing

// app.MapGet("api/products", () => Results.Ok("all products"));
// app.MapGet("api/products/{id:int}", (int id) => Results.Ok($"product with id({id})"));
// app.MapPost("api/products", (ProductCreateRequestDto request) => 
// Results.Created(string.Empty, $"created product with name({request.Name})"));
// app.MapPut("api/products", (ProductUpdateRequestDto request) => Results.NoContent());
// app.MapDelete("api/products/{id:int}", (int id) => Results.NoContent()); 

#endregion

app.Run();