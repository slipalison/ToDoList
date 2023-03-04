using System.Text.Json;
using Domain.Commands;
using Domain.Contracts.Services;
using Infra.MassTransitConfiguration.Consumers;
using Infra.MassTransitConfiguration.Publishers;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infra.MassTransitConfiguration;

public static class MasstransitExtension
{
    public static IServiceCollection AddMassTransitWithRabbitMq(this IServiceCollection services, IConfiguration configuration)
    {

        services.Configure<MassTransitHostOptions>(options =>
        {
            options.WaitUntilStarted = true; 
            options.StartTimeout = TimeSpan.FromSeconds(30);
            options.StopTimeout = TimeSpan.FromMinutes(1);
        });

        
        return services.AddToDoListHost(configuration).AddScoped(typeof(IToDoListPublisher<>), typeof(ToDoListPublisher<>));
    }

    private static IServiceCollection AddToDoListHost(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IConsumer<ToDoItemQueueCreateCommand>,ToDoConsumer>();

        if (configuration["environment"] == "Test")
            return services;
        
        return services.AddMassTransit<IToDoListBus>(cfg =>
        {
            
            cfg.AddConsumer<ToDoConsumer, ToDoConsumerDefinition>();
            
            cfg.UsingRabbitMq((context, configurator) =>
            {
                
                
                configurator.AddFanOutPublisher<ToDoItemQueueCreateCommand>("Create.Exchange");
                configurator.CopnfigurationRabbit(configuration);
                configurator.ConfigureEndpoints(context);
            });
            
        });
    }

    private static void CopnfigurationRabbit(this IRabbitMqBusFactoryConfigurator cfg, IConfiguration configuration)
    {
        var pass = configuration["RabbitMq:password"];
        var user = configuration["RabbitMq:user"];

        Console.WriteLine("Value: " + configuration.GetSection("RabbitMq:hosts").Value);
        var hosts = 
        JsonSerializer.Deserialize<List<string>>(configuration.GetSection("RabbitMq:hosts").Value!);

        var maimHost = hosts.First().Split(":");
        
        
        cfg.Host(maimHost[0], ushort.Parse(maimHost[1]), "/", (hostCongig) =>
        {
            hostCongig.Username(user);
            hostCongig.Password(pass);
            hostCongig.PublisherConfirmation = true; 
            hostCongig.UseCluster(c=> hosts.ForEach(c.Node));
        });
    }
}