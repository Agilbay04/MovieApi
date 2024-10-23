using MovieApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

//** Add services to the container **//

builder.ConfigureLogging();
builder.ConfigureSentry();
builder.ConfigureEnvironment();
builder.ConfigureDatabase();
builder.ConfigureJwt();
builder.ConfigureSwagger();
builder.ConfigureCors();
builder.ConfigureControllers();
builder.Services.AddApplicationServices();

var app = builder.Build();

await app.SeedDatabaseAsync();
app.ConfigureMiddleware();
await app.ConfigureSchedulerAsync();
app.Run();
