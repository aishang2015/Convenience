using Microsoft.Extensions.Configuration;

namespace Convience.WPFClient.Utils
{
    public static class ConfigurationUtil
    {
        private static readonly IConfigurationRoot _configurationRoot;

        static ConfigurationUtil()
        {
            if (_configurationRoot == null)
            {
                _configurationRoot = new ConfigurationBuilder().AddJsonFile("App.json").Build();
            }
        }

        public static string GetConnectionString(string connectionName)
        {
            return _configurationRoot[$"ConnectionStrings:{connectionName}"];
        }

        public static string GetBaseUri()
        {
            return _configurationRoot[$"Uri:BaseUri"];
        }

        public static string GetApiUri()
        {
            return _configurationRoot[$"Uri:ApiUri"];
        }
    }
}
