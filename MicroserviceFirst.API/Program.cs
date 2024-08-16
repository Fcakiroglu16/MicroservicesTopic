using MicroserviceFirst.API;
using Polly;
using Polly.Extensions.Http;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Polly politikasını yapılandırıyoruz
var retryPolicy = HttpPolicyExtensions
    .HandleTransientHttpError() // HTTP 5xx, 408 ve network hatalarını yakalar
    /*.OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)*/ // 404 hatalarını da yakalar
    .WaitAndRetryAsync(3,
        retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))); // 3 kez dene, aradaki süreyi arttır

var circuitBreakerPolicy = HttpPolicyExtensions
    .HandleTransientHttpError()
    .CircuitBreakerAsync(2, TimeSpan.FromMinutes(1)); // 2 hata sonrası 1 dakika devreye girmez

var timeoutPolicy = Policy.TimeoutAsync<HttpResponseMessage>(5);


var combinedPolicy = Policy.WrapAsync(retryPolicy, circuitBreakerPolicy, timeoutPolicy);


builder.Services.AddHttpClient<MicroserviceSecondService>(configure =>
{
    configure.BaseAddress =
        new Uri(builder.Configuration.GetSection("MicroserviceBaseUrls")["MicroserviceSecond"]!);
}).AddPolicyHandler(combinedPolicy);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapGet("/api/SendRequestToMicroserviceTwo",
    async (MicroserviceSecondService secondMicroserviceService) =>
    {
        var response = await secondMicroserviceService.GetProducts();


        return Results.Ok(response);
    }).WithName("SendRequestToMicroserviceTwo").WithOpenApi();

app.Run();