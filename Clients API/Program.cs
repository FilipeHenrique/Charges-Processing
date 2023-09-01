using Clients_API.Middlewares;
using Domain.Clients.Entities;
using Domain.Clients.Interfaces;
using Domain.Clients.UseCases;
using Infrastructure.DbContext;
using Infrastructure.Repositories;
using Infrastructure.Services;
using MongoDB.Driver;
using System.Collections;

var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.

//// Register MongoDB components from Infrastructure
var configuration = builder.Configuration;
var connectionString = configuration.GetSection("ChargesDatabase:ConnectionString").Value;
var databaseName = configuration.GetSection("ChargesDatabase:DatabaseName").Value;
builder.Services.AddSingleton<IDBContext>(new DBContext(connectionString, databaseName));

/// Common Services
builder.Services.AddTransient<ICPFValidationService, CPFValidationService>();

/// Repositories
builder.Services.AddTransient<IRepository<Client>>(a =>
{
    var database = a.GetService<IDBContext>();
    return new ClientsRepository<Client>(database, "Clients");
});

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

app.UseMiddleware<ErrorHandlerMiddleware>();

app.MapControllers();

app.Run();
