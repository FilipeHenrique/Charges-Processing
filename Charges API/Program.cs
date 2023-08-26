using Domain.Charges.Interfaces.Repositories;
using Domain.Charges.UseCases;
using Infrastructure.DbContext;
using Infrastructure.Repositories;
using Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//// Register MongoDB components from Infrastructure
var configuration = builder.Configuration;
var connectionString = configuration.GetSection("ChargesDatabase:ConnectionString").Value;
var databaseName = configuration.GetSection("ChargesDatabase:DatabaseName").Value;
builder.Services.AddSingleton<IMongoDBContext>(new MongoDBContext(connectionString, databaseName));

/// Common Services
builder.Services.AddTransient<ICPFValidationService, CPFValidationService>();

/// Repositories
builder.Services.AddTransient<IChargesRepository, ChargesRepository>();

/// Use Cases
builder.Services.AddUseCases();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
