using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Server.Extensions.Services
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
                            .AllowCredentials()
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
                            policy.AllowAnyMethod()
                                .AllowAnyHeader()
                                .AllowCredentials()
                                .SetIsOriginAllowed(hostName => true);
                            
                        });
                    c.AddPolicy("CorsPolicy",
                        policy => {
                            policy
                            .AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader();
                        });
                }

            });
        }
    }
}
