using FaceDetectionAPI.Models;
using FaceDetectionAPI.Services;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Bind config from appsettings.json
builder.Services.Configure<OllamaConfig>(builder.Configuration.GetSection("Ollama"));

// Register HttpClient with dependency injection
builder.Services.AddHttpClient<OllamaFaceVerificationService>((serviceProvider, client) =>
{
    var config = serviceProvider.GetRequiredService<IOptions<OllamaConfig>>().Value;
    client.BaseAddress = new Uri(config.BaseUrl);
});

builder.Services.AddScoped<FaceDetectionService>();
//builder.Services.AddScoped<OllamaFaceVerificationService>();

builder.Services.AddEndpointsApiExplorer();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseRouting();
app.UseAuthorization();
app.MapControllers();
app.Run();

