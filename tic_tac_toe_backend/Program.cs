using dotnet.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument(settings =>
{
    settings.Title = "Tic Tac Toe API";
    settings.Description = "Simple REST API for Tic Tac Toe (in-memory).";
    settings.Version = "v1";
});
builder.Services.AddControllers();

// Add GameService (in-memory singleton)
builder.Services.AddSingleton<GameService>();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.SetIsOriginAllowed(_ => true)
              .AllowCredentials()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Use CORS
app.UseCors("AllowAll");

// Configure OpenAPI/Swagger
app.UseOpenApi();
app.UseSwaggerUi(config =>
{
    config.Path = "/docs";
});

// Map controllers
app.MapControllers();

// Health check endpoint
app.MapGet("/", () => new { message = "Healthy" });

app.Run();