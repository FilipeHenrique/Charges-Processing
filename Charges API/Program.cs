using Application.UseCases.Charges;
using Domain.Contracts.Repositories;
using Domain.Contracts.UseCases.Charges;
using Domain.Services;
using Infrastructure.DbContext;
using Infrastructure.Repositories;
using Infrastructure.Services.Charges_Processing_Job;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//// Register MongoDB components from Infrastructure
var configuration = builder.Configuration;
var connectionString = configuration.GetSection("ChargesDatabase:ConnectionString").Value;
var databaseName = configuration.GetSection("ChargesDatabase:DatabaseName").Value;
builder.Services.AddSingleton<IMongoDBContext>(new MongoDBContext(connectionString, databaseName));

/// Common Services
builder.Services.AddInfrastructure();
builder.Services.AddTransient<ICPFValidationService, CPFValidationService>();

/// Repositories
builder.Services.AddTransient<IChargesRepository, ChargesRepository>();

/// Use Cases
builder.Services.AddTransient<ICreateChargeUseCase, CreateChargeUseCase>();
builder.Services.AddTransient<IListChargesUseCase, ListChargesUseCase>();

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
