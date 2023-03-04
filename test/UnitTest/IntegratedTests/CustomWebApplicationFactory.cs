using System.Data.Common;
using Infra.Databases.SqlServers.ToDoListData;
using Infra.MassTransitConfiguration;
using Infra.MassTransitConfiguration.Consumers;
using MassTransit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;


namespace UnitTest.IntegratedTests;


public class CustomWebApplicationFactory<TProgram>
    : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.AddMassTransitTestHarness(cfg =>
            {
                cfg.AddConsumer<ToDoConsumer, ToDoConsumerDefinition>();
            });

            services.AddScoped<IToDoListBus>(x=>Substitute.For<IToDoListBus>());
            MockInMemoryDatabase(services);
        });
        
        //app.ApplicationServices.GetRequiredService<ITestHarness>().Start();
        
        builder.UseEnvironment("Test");
    }

    private static void MockInMemoryDatabase(IServiceCollection services)
    {
        var dbContextDescriptor = services.SingleOrDefault(
            d => d.ServiceType ==
                 typeof(DbContextOptions<ToDoListContext>));

        if (dbContextDescriptor != null) services.Remove(dbContextDescriptor);

        var dbConnectionDescriptor = services.SingleOrDefault(
            d => d.ServiceType ==
                 typeof(DbConnection));

        if (dbConnectionDescriptor != null) services.Remove(dbConnectionDescriptor);


        services.AddSingleton<DbConnection>(container =>
        {
            var connection = new SqliteConnection("Data Source=:memory:");
            connection.Open();

            return connection;
        });

        services.AddDbContextPool<ToDoListContext>((container, options) =>
        {
            var connection = container.GetRequiredService<DbConnection>();
            options.UseSqlite(connection);
        });
    }
}