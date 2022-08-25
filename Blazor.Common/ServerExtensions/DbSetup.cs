using Blazor.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Blazor.Common.ServerExtensions
{
    public static class DbSetup
    {
        public static void AddEntityFramework(this IServiceCollection services, IConfiguration configuration)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            if (configuration["UseDb"] == "MySql")
            {
                services.AddDbContext<TodoContext>(options =>
                        options.UseMySql(configuration.GetConnectionString("MySqlConnection"), new MySqlServerVersion(new Version(5, 7)))
                        .LogTo(Console.WriteLine)
                        .EnableSensitiveDataLogging()
                        .EnableDetailedErrors());
            }
            else
            {
                services.AddDbContext<TodoContext>(options =>
                        options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")).LogTo(Console.WriteLine));
            }

        }
    }
}
