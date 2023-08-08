using Application.UseCases;
using Domain.Contracts.Repositories;
using Domain.Contracts.UseCases;
using Domain.Services;
using Infrastructure.DbContext;
using Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.

//// Register MongoDB components from Infrastructure
var configuration = builder.Configuration;
var connectionString = configuration.GetSection("ChargesDatabase:ConnectionString").Value;
var databaseName = configuration.GetSection("ChargesDatabase:DatabaseName").Value;
builder.Services.AddSingleton<IMongoDBContext>(new MongoDBContext(connectionString, databaseName));

/// Common Services
builder.Services.AddScoped<ICPFValidationService, CPFValidationService>();

/// Repositories
builder.Services.AddSingleton<ICreateClientRepository, CreateClientRepository>();
builder.Services.AddSingleton<IGetClientRepository, GetClientRepository>();

/// Use Cases
builder.Services.AddScoped<ICreateClientUseCase, CreateClientUseCase>();
builder.Services.AddScoped<IGetClientUseCase, GetClientUseCase>();

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
