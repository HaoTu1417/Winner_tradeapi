using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using NLog;
using NLog.Web;
using System;
using System.Data;
using System.IO;
using System.Runtime.InteropServices;
using tradeapi.Utility;
using tradeapi2.Middleware;

#nullable enable

// Configure NLog
// var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
Logger logger = NLog.SetupBuilderExtensions.GetCurrentClassLogger(NLog.Web.SetupBuilderExtensions.LoadConfigurationFromAppSettings(LogManager.Setup()));
try
{
    logger.Info("Starting Trade API...");

    // Create builder
    var builder = WebApplication.CreateBuilder(args);
    var configuration = builder.Configuration;

    // ✅ Fix: Properly use NLog
    builder.Host.UseNLog();

    // ✅ Fix: Register CORS policy
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAll", policy =>
        {
            policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
        });
    });

    // ✅ Fix: Initialize Database Connections
    DapperMysql.Init(configuration.GetConnectionString("MySql") ?? throw new Exception("MySql connection not found"));
    StockDb.Init(configuration.GetConnectionString("StockDb") ?? throw new Exception("StockDb connection not found"));

    // ✅ Fix: Register Middleware & Controllers
    builder.Services.AddSingleton<ApiKeyMiddleware>();
    builder.Services.AddControllers(); // Correct way to register controllers

    // ✅ Fix: Register Swagger (Only on Windows/macOS)
    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) || RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
    {
        string xmlFile = "apidoc.xml";
        string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Trade API",
                Version = "v1"
            });
            
            // Add the "token" header as a security scheme
            options.AddSecurityDefinition("token", new OpenApiSecurityScheme
            {
                Description = "Custom token header",
                Name = "token", // name of the header
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "token"
            });
            
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "token"
                        }
                    },
                    new string[] {}
                }
            });


            if (File.Exists(xmlPath))
            {
                options.IncludeXmlComments(xmlPath);
            }
        });
    }

    var app = builder.Build();

    // ✅ Fix: Enable Swagger UI in Development Mode
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Trade API v1");
        });
    }

    // ✅ Fix: Enable Middleware & Routing
    app.UseCors("AllowAll");
    app.UseAuthentication();
    app.UseAuthorization();
    app.UseMiddleware<ApiKeyMiddleware>();
    app.MapControllers();

    // ✅ Fix: Define API listening URL properly
    app.Urls.Add("http://0.0.0.0:5278");

    // ✅ Fix: Run the application correctly
    app.Run();
}
catch (Exception ex)
{
    logger.Error(ex, "Application stopped due to an exception.");
    throw;
}
finally
{
    LogManager.Shutdown();
}
