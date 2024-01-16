using MicroserviceFirst.API;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient<MicroserviceSecondService>(configure => configure.BaseAddress = new Uri(builder.Configuration.GetSection("MicroserviceBaseUrls")["MicroserviceSecond"]!));




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