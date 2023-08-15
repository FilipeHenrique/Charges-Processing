using Application.UseCases.Charges;
using Domain.Contracts.Repositories.Charges;
using Domain.Contracts.UseCases.Charges;
using Domain.Services;
using Infrastructure.DbContext;
using Infrastructure.Repositories.Charges;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//// Register MongoDB components from Infrastructure
var configuration = builder.Configuration;
var connectionString = configuration.GetSection("ChargesDatabase:ConnectionString").Value;
var databaseName = configuration.GetSection("ChargesDatabase:DatabaseName").Value;
builder.Services.AddSingleton<IMongoDBContext>(new MongoDBContext(connectionString, databaseName));

// Configure Mongo ClassMapping
MongoDBConfig.Configure();

/// Common Services
builder.Services.AddScoped<ICPFValidationService, CPFValidationService>();

/// Repositories
builder.Services.AddSingleton<ICreateChargeRepository, CreateChargeRepository>();
builder.Services.AddSingleton<IGetChargesRepository, GetChargesRepository>();

/// Use Cases
builder.Services.AddScoped<ICreateChargeUseCase, CreateChargeUseCase>();
builder.Services.AddScoped<IGetChargesUseCase, GetChargesUseCase>();

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
