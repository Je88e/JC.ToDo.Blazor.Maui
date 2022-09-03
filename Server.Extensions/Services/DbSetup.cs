using Blazor.Entity.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Server.Extensions.Config;

namespace Server.Extensions.Services
{
    public static class DbSetup
    {
        public static void AddEntityFramework(this IServiceCollection services, IConfiguration configuration)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            if (configuration["UseDb"] == "MySql")
            {
                services.AddDbContext<TodoContext>(options =>
                        options.UseMySql(SecretConfig.Db_MySql_Secret_ConnectString, new MySqlServerVersion(new Version(5, 7)))
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
