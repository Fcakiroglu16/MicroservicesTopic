using MicroserviceSecond.API.Products;

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


app.MapGroup("/api/products").MapProductApi().WithTags("Products Api");

#region legacy routing

// app.MapGet("api/products", () => Results.Ok("all products"));
// app.MapGet("api/products/{id:int}", (int id) => Results.Ok($"product with id({id})"));
// app.MapPost("api/products", (ProductCreateRequestDto request) => 
// Results.Created(string.Empty, $"created product with name({request.Name})"));
// app.MapPut("api/products", (ProductUpdateRequestDto request) => Results.NoContent());
// app.MapDelete("api/products/{id:int}", (int id) => Results.NoContent()); 

#endregion

app.Run();