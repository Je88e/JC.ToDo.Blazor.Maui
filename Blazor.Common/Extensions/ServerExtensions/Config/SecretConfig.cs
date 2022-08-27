using Microsoft.Extensions.Configuration;

namespace Blazor.Common.Extensions.ServerExtensions.Config
{
    public class SecretConfig
    {
        private static IConfiguration configuration;
        public SecretConfig(IConfiguration _configuration)
        {
            configuration = _configuration;
        }

        public static string Jwt_Secret_File => ReadSecretFile(configuration.GetSection("Secret").GetSection("File")["JwtSecretFile"]);
        public static string Db_MySql_Secret_ConnectString => ReadSecretFile(configuration.GetSection("Secret").GetSection("Value")["MySqlSecretConnectString"]);

        private static string ReadSecretFile(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    return File.ReadAllText(filePath).Trim();
                }
            }
            catch (Exception) { }

            return "";
        }
    }
}
