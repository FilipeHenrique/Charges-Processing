using Application.UseCases.Clients;
using Domain.Contracts.Repositories.Clients;
using Domain.Contracts.UseCases.Clients;
using Domain.Services;
using Infrastructure.DbContext;
using Infrastructure.Repositories.Clients;

var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.

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
builder.Services.AddSingleton<ICreateClientRepository, CreateClientRepository>();
builder.Services.AddSingleton<IGetClientsRepository, GetClientsRepository>();

/// Use Cases
builder.Services.AddScoped<ICreateClientUseCase, CreateClientUseCase>();
builder.Services.AddScoped<IGetClientsUseCase, GetClientsUseCase>();

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
