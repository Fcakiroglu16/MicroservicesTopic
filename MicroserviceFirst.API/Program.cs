using MicroserviceFirst.API;

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
}

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/api/products/create", () => Results.Ok(Environment.GetEnvironmentVariable("Instance")));
app.MapGet("/api/products", () => Results.Ok(new List<string>() { "kalem 1", "kalem 2" }));

app.Run();