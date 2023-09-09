using Domain.Clients.Entities;
using Infrastructure.DbContext;
using Infrastructure.Middlewares;
using Infrastructure.Repositories;
using Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.

//// Register MongoDB components from Infrastructure
var configuration = builder.Configuration;
var connectionString = configuration.GetSection("ChargesDatabase:ConnectionString").Value;
var databaseName = configuration.GetSection("ChargesDatabase:DatabaseName").Value;
builder.Services.AddSingleton<IDBContext>(new DBContext(connectionString, databaseName));

/// Common Services
builder.Services.AddTransient<ICPFHandler, CPFHandler>();

/// Repositories
builder.Services.AddTransient<IRepository<Client>>(provider =>
{
    var database = provider.GetService<IDBContext>();
    return new ClientsRepository<Client>(database, "Clients");
});

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

app.AddErrorHandlingMiddleware();

app.MapControllers();

app.Run();
