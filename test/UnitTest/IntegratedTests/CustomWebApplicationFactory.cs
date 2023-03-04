using System.Data.Common;
using Infra.Databases.SqlServers.ToDoListData;
using Infra.MassTransitConfiguration;
using MassTransit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MassTransit.Testing;


namespace UnitTest.IntegratedTests;

public class CustomWebApplicationFactory<TProgram>
    : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        
        var harness = new InMemoryTestHarness($"loopback://localhost:5672/");
        harness.Start().GetAwaiter().GetResult();
        
        
        var bus = Bus.Factory.CreateUsingInMemory(cfg =>
        {
         
        });
        
      
        
        builder.ConfigureServices(services =>
        {
            services.AddSingleton<IBus>(bus);
            
            
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
        });

        builder.UseEnvironment("Test");
    }
}