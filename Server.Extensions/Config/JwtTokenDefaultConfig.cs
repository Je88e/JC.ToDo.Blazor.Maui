using Microsoft.Extensions.Configuration;

namespace Server.Extensions.Config
{
    public class JwtTokenDefaultConfig
    {
        private static IConfiguration configuration;
        public JwtTokenDefaultConfig(IConfiguration _configuration)
        {
            configuration = _configuration;
        }


        public static string Issuer => configuration.GetSection("Audience")["Issuer"];
        public static string Audience => configuration.GetSection("Audience")["Audience"];

    }
}
