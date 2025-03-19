// Decompiled with JetBrains decompiler
// Type: Program
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: C:\Users\VN6\Documents\Projects\Winner Dotnet\service\tradeapi\tradeapi.dll

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.OpenApi.Models;
using NLog;
using NLog.Web;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Data;
using System.IO;
using System.Runtime.InteropServices;
using tradeapi.Middleware;
using tradeapi.Utility;

#nullable enable
Logger currentClassLogger = NLog.SetupBuilderExtensions.GetCurrentClassLogger(NLog.Web.SetupBuilderExtensions.LoadConfigurationFromAppSettings(LogManager.Setup()));
try
{
  WebApplicationBuilder builder1 = WebApplication.CreateBuilder(args);
  ConfigurationManager configuration = builder1.Configuration;
  // Microsoft.Extensions.Logging.LoggingBuilderExtensions.ClearProviders(builder1.Logging);
  AspNetExtensions.UseNLog((IHostBuilder) builder1.Host);
  CorsServiceCollectionExtensions.AddCors(builder1.Services, (Action<CorsOptions>) (options => options.AddPolicy("AllowAll", (Action<CorsPolicyBuilder>) (builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()))));
  DapperMysql.Init(ConfigurationExtensions.GetConnectionString((IConfiguration) configuration, "MySql") ?? throw new Exception("找不到MySql連線設定"));
  StockDb.Init(ConfigurationExtensions.GetConnectionString((IConfiguration) configuration, "StockDb") ?? throw new Exception("找不到StockDb連線設定"));
  ServiceCollectionServiceExtensions.AddSingleton<ApiKeyMiddleware>(builder1.Services);
  MvcServiceCollectionExtensions.AddControllers(builder1.Services);
  if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
  {
    string filePath2 = Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, "apidoc.xml");
    EndpointMetadataApiExplorerServiceCollectionExtensions.AddEndpointsApiExplorer(builder1.Services);
    SwaggerGenServiceCollectionExtensions.AddSwaggerGen(builder1.Services, (Action<SwaggerGenOptions>) (options =>
    {
      string filePath = Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, "apidoc.xml");
      SwaggerGenOptionsExtensions.IncludeXmlComments(options, filePath);
      SwaggerGenOptionsExtensions.SwaggerDoc(options, "v1", new OpenApiInfo()
      {
        Title = "Trade Api",
        Version = "v1"
      });
    }));
  }
  WebApplication app = builder1.Build();
  if (HostEnvironmentEnvExtensions.IsDevelopment((IHostEnvironment) app.Environment))
  {
    SwaggerBuilderExtensions.UseSwagger((IApplicationBuilder) app);
    SwaggerUIBuilderExtensions.UseSwaggerUI((IApplicationBuilder) app);
  }
  CorsMiddlewareExtensions.UseCors((IApplicationBuilder) app, "AllowAll");
  AuthAppBuilderExtensions.UseAuthentication((IApplicationBuilder) app);
  AuthorizationAppBuilderExtensions.UseAuthorization((IApplicationBuilder) app);
  UseMiddlewareExtensions.UseMiddleware<ApiKeyMiddleware>((IApplicationBuilder) app, Array.Empty<object>());
  ControllerEndpointRouteBuilderExtensions.MapControllers((IEndpointRouteBuilder) app);
  IDbConnection writeConntion = DapperMysql.GetWriteConntion();
  app.Urls.Add("http://0.0.0.0:5278");
  app.Run((string) null);
 
}
catch (Exception ex)
{
  currentClassLogger.Error(ex, "Stopped program because of exception");
  throw;
}
finally
{
  LogManager.Shutdown();
}
