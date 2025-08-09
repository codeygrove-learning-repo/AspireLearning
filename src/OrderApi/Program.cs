var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults(); // Adds common .NET Aspire services: service discovery, resilience, health checks, and OpenTelemetry.

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.Configure<AspireLearning.Repository.MongoDbSettings>(builder.Configuration.GetSection("MongoDb"));
builder.Services.AddControllers();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy
        // Origin's port need to match the port of AddNpmApp in AspireLearning.AppHost
        // Because it's a React app running on localhost:51733
        //.WithOrigins("http://localhost:51733")        
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

var app = builder.Build();

app.MapDefaultEndpoints(); // Add Aspire Orchestration default endpoints

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseCors(); // <-- Add this line
app.MapControllers();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
