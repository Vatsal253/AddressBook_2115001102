using FluentValidation.AspNetCore;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using NLog;
using NLog.Web;
using RepositoryLayer.Context;
using System;

var logger = LogManager.Setup().LoadConfigurationFromFile("nlog.config").GetCurrentClassLogger();
try
{
    logger.Info("Application is starting...");
    var builder = WebApplication.CreateBuilder(args);

// Database Connection
var connectionString = builder.Configuration.GetConnectionString("SqlConnection");
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

// Add services to the container.
builder.Services.AddControllers();
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
    // Add AutoMapper
 builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

    // Add FluentValidation
    builder.Services.AddFluentValidationAutoValidation();
    builder.Services.AddValidatorsFromAssemblyContaining<AddressBookEntryModelValidator>(); // Ensure Validator is correctly registered



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
}
catch (Exception ex)
{
    logger.Error(ex, "Application startup failed.");
    throw;
}
finally
{
    LogManager.Shutdown();
}