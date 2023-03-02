using Domain.Contracts.Repositories;
using Infra.Databases.SqlServers.ToDoListData.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infra.Databases.SqlServers.ToDoListData.Extensions;

public static class IncludeUCondoConnection
{
    public static IServiceCollection AddUCondoContext(this IServiceCollection serviceCollection,
        IConfiguration configuration)
    {
        serviceCollection
            .AddDbContextPool<ToDoListContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("SqlServer"))).AddRepositories();

        return serviceCollection;
    }

    private static void AddRepositories(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IToDoListRepository, ToDoListRepository>();
    }

    public static void ExecuteMigartions(this IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
        var dbContext = serviceScope.ServiceProvider.GetRequiredService<ToDoListContext>();
        dbContext.Database.EnsureCreated();
    }
}