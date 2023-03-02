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

        //services.AddScoped<>() consumer

        return services.AddToDoListHost(configuration);
    }

    private static IServiceCollection AddToDoListHost(this IServiceCollection services, IConfiguration configuration)
    {
        return services.AddMassTransit<IToDoListBus>(cfg =>
        {
            cfg.UsingRabbitMq((context, configurator) =>
            {
                configurator.CopnfigurationRabbit(configuration);
                configurator.ConfigureEndpoints(context);
            });
            
        });
    }

    private static void CopnfigurationRabbit(this IRabbitMqBusFactoryConfigurator cfg, IConfiguration configuration)
    {
        var pass = configuration["RabbitMq:password"];
        var user = configuration["RabbitMq:user"];

        var hosts = new List<string>();
        configuration.GetSection("RabbitMq:hosts").Bind(hosts);

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