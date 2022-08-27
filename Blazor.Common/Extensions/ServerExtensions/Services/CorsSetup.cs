using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Blazor.Common.Extensions.ServerExtensions.Services
{
    /// <summary>
    /// Cors 启动服务
    /// </summary>
    public static class CorsSetup
    {
        public static void AddCorsSetup(this IServiceCollection services, IConfiguration configuration)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            IConfigurationSection policyConfiguration = configuration.GetSection("Cors");
            services.AddCors(c =>
            {
                bool allowAnyIp = Convert.ToBoolean(policyConfiguration["EnableAllIPs"]);
                string policyName = policyConfiguration["PolicyName"];
                if (!allowAnyIp)
                {
                    c.AddPolicy(policyName,

                        policy =>
                        {
                            policy
                            .WithOrigins(policyConfiguration["IPs"].Split(','))
                            .AllowAnyHeader()//Ensures that the policy allows any header.
                            .AllowAnyMethod();
                        });
                }
                else
                {
                    //允许任意跨域请求
                    c.AddPolicy(policyName,
                        policy =>
                        {
                            policy.AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .AllowCredentials();
                        });
                }

            });
        }
    }
}
