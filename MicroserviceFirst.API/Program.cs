using MicroserviceFirst.API;
using MicroserviceFirst.API.Redis;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient<MicroserviceSecondService>(configure =>
    configure.BaseAddress = new Uri(builder.Configuration.GetSection("MicroserviceBaseUrls")["MicroserviceSecond"]!));



builder.Services.AddOptions<RedisOption>().BindConfiguration(nameof(RedisOption))
    .ValidateDataAnnotations()
    .ValidateOnStart();
builder.Services.AddSingleton(sp => sp.GetRequiredService<IOptions<RedisOption>>().Value);

builder.Services.AddSingleton(sp =>
{
    var redisOptions = sp.GetRequiredService<RedisOption>();

    var logger = sp.GetRequiredService<ILogger<RedisService>>();

    return new RedisService(redisOptions, logger);
});

var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    
    
    var redisService = scope.ServiceProvider.GetRequiredService<RedisService>();
  
    
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



app.MapGet("/api/SendRequestToMicroserviceTwo",
    async (MicroserviceSecondService secondMicroserviceService) =>
    {
        var response = await secondMicroserviceService.GetProducts();


        return Results.Ok(response);
    }).WithName("SendRequestToMicroserviceTwo").WithOpenApi();



app.MapGet("/api/redis", (RedisService redisService) =>
    {

        redisService.GetDb().StringSet("key1", "key1-value");

        var value = redisService.GetDb().StringGet("key1");


        return Task.FromResult(Results.Ok(value.ToString()));
    }).WithName("redis").WithOpenApi();

app.Run();