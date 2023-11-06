using Microsoft.Extensions.Configuration;

namespace ProductCatalog.Utility.Helpers
{
    public class ConfigurationUtility
    {
        private IConfigurationRoot _configuration;

        public ConfigurationUtility(IConfigurationRoot configuration)
        {
            _configuration = configuration;
        }

        public string GetValue(string key)
        {
            return _configuration[key];
        }
    }
}
