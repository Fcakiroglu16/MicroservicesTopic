using MassTransit;
using MicroserviceFirst.API;
using SharedEvents;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddMassTransit(configure =>
{
    configure.AddRequestClient<UserCreatedEvent>();

    configure.UsingRabbitMq((context, rabbitConfigure) => { rabbitConfigure.Host("localhost", "/", x => { }); });
});

#region in-memory use case

//builder.Services.AddMassTransit(configure =>
//{
//    configure.AddConsumer<UserCreatedEventConsumer>();

//    configure.UsingInMemory((context, cfg) => { cfg.ConfigureEndpoints(context); });
//});

#endregion

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

app.MapGet("/api/request-response-message-pattern",
    async (IRequestClient<UserCreatedEvent> requestClient) =>
    {
        var result = await requestClient.GetResponse<UserCreatedEventResult>(new UserCreatedEvent("ahmet"));


        return Results.Ok(result.Message.Id);
    });

app.MapGet("/api/SendRequestToMicroserviceTwo",
    async (MicroserviceSecondService secondMicroserviceService) =>
    {
        var response = await secondMicroserviceService.GetProducts();

        return Results.Ok(response);
    }).WithName("SendRequestToMicroserviceTwo").WithOpenApi();

app.Run();