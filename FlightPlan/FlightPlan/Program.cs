using System.Configuration;
using FlightPlan.Data;
using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();
builder.Services.AddScoped<IDatabaseAdapter , MongoDbDatabase>();

var app = builder.Build();


// Access the configuration
var configuration = new ConfigurationBuilder()
    .SetBasePath(app.Environment.ContentRootPath)
    .AddJsonFile("appsettings.Development.json", optional: false, reloadOnChange: true)
    .Build();

var connectionString = configuration.GetConnectionString("MongoDB"); // Get the connection string from appsettings.json
var client = new MongoClient(connectionString);

if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new ApplicationException("MongoDB connection string is missing or empty in the configuration.");
}
var database = client.GetDatabase("Vineetrai");


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Accept Any http.verb to use.
app.UseCors(config =>
{
    config
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader();

});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

