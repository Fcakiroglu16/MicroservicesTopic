using Composition.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient<ProductServices>(x => { x.BaseAddress = new Uri("https://localhost:7082"); });

builder.Services.AddHttpClient<StockServices>(x => { x.BaseAddress = new Uri("https://localhost:7173"); });

builder.Services.AddScoped<CompositeService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapGet("api/products-composite/{id}",
    async (int id, CompositeService compositeService) =>
    {
        var result = await compositeService.GetProductWithFull(id);

        if (result is null)
        {
            return Results.NotFound();
        }

        return Results.Ok(result);
    });


app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}