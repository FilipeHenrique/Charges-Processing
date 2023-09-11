using Domain.Charges.Entities;
using Infrastructure.Database;
using Infrastructure.Middlewares.ErrorHandler;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//// Register MongoDB components from Infrastructure
builder.Services.AddDbContext<DBContext<Charge>>(options =>
{
    options.UseInMemoryDatabase("Charges");
});

/// Common Services
builder.Services.AddTransient<ICPFHandler, CPFHandler>();

/// Repositories
builder.Services.AddTransient<IRepository<Charge>>(provider =>
{
    var dbContext = provider.GetRequiredService<DBContext<Charge>>();
    return new ChargesRepository<Charge>(dbContext);
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
