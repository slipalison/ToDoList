using System.Text.Json;
using System.Text.Json.Serialization;
using Domain.Contracts.Services;
using Domain.Services;
using Infra.ConfigsExtensions;
using Infra.Databases.SqlServers.ToDoListData.Extensions;
using Infra.MassTransitConfiguration;
using Infra.Middlewares;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.OpenApi.Models;
using Serilog;

namespace WebApi;

public class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            });

        services.AddCors(policyBuilder =>
            policyBuilder.AddDefaultPolicy(policy =>
                policy.AllowAnyHeader().AllowAnyHeader())
        );

        services.AddResponseCompression(options =>
        {
            options.Providers.Add<GzipCompressionProvider>();
            options.EnableForHttps = true;
        });

        services.AddEndpointsApiExplorer()
            .AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Base API", Version = "v1" });
                    c.UseInlineDefinitionsForEnums();
                }
            );

        services.AddScoped<ICorrelationContextService, CorrelationContextService>();

        services.AddMassTransitWithRabbitMq(_configuration)
            .HealthChecksConfiguration(_configuration)
            .AddUCondoContext(_configuration)
            .AddDomainServices();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseCors(builder => builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
        app.UseMiddleware<CorrelationMiddleware>().UseMiddleware<LoggingMiddleware>();
        app.UseSerilogRequestLogging();
        // TODO: Isso é só para testes e apresentação, hambiente produtivo isso é contra indicado
        app.ExecuteMigartions();

        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseHttpsRedirection().UseRouting()
            .UseResponseCompression()
            .UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            })
            .UseAuthorization()
            .HealthCheckConfiguration();
    }
}