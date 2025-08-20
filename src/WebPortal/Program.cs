var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults(); // Adds common .NET Aspire services: service discovery, resilience, health checks, and OpenTelemetry.

// Add services to the container.
builder.Services.AddControllersWithViews();

// "aspirelearning-catalogapi" is the friendly name specify in AspireLearning.AppHost
builder.Services.AddHttpClient("aspirelearning-catalogapi", httpClient =>
{
    httpClient.BaseAddress = new Uri("https+http://aspirelearning-catalogapi");
});

// "aspirelearning-orderapi" is the friendly name specify in AspireLearning.AppHost
builder.Services.AddHttpClient("aspirelearning-orderapi", httpClient =>
{
    httpClient.BaseAddress = new Uri("https+http://aspirelearning-orderapi");
});

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
