using MicroserviceFirst.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


#region Read From AppSettings.json Configurations

//builder.Services.Configure<ExampleOptions>(
//    builder.Configuration.GetSection("Example")
//); 

#endregion


builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
});


builder.Configuration.Add<SqlServerConfigurationSource>(source =>
{
    source.ConnectionString = builder.Configuration.GetConnectionString("SqlServer");
});

builder.Services.Configure<AppSettingsConfiguration>(builder.Configuration.GetSection("Keys"));
builder.Services.Configure<MicroserviceUrlsConfiguration>(builder.Configuration.GetSection("MicroserviceBaseUrls"));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>()!;

    if (!dbContext.AppSettings.Any())
    {
        dbContext.AppSettings.Add(new() { Key = "Keys:SmsKey", Value = "SmsKey value" });
        dbContext.SaveChanges();
    }
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/api/ConfigurationFromSqlserver",
    (IOptions<AppSettingsConfiguration> options, IOptions<MicroserviceUrlsConfiguration> options2) =>
    {
        var smsKey = options.Value.SmsKey;

        var microserviceSecond = options2.Value.MicroserviceSecond;


        return Results.Ok(new List<string>() { smsKey, microserviceSecond });
    }).WithName("SendRequestToMicroserviceTwo").WithOpenApi();
app.Run();