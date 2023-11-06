using ProductCatalog.Services.Abstractions;
using ProductCatalog.Utility.Enums;
using ProductCatalog.Utility.Extensions;
using ProductCatalog.Utility.Helpers;

namespace ProductCatalog.Infrastructure.Business.Common
{
    public class NationalBankApiService : IApiService
    {
        private HttpUtility _httpUtility { get; set; }
        private ConfigurationUtility _configurationUtility { get; set; }
        public NationalBankApiService(HttpUtility httpUtility, ConfigurationUtility configurationUtility)
        {
            this._httpUtility = httpUtility;
            this._configurationUtility = configurationUtility;
        }

        public async Task<T> ExecuteRequest<T>(Enum command, params object[] parameters)
        {
            var url = default(string);
            switch ((NationalBankAction)command)
            {
                case NationalBankAction.GetCurrencies:
                {
                    url = $"{this._configurationUtility.GetValue("NBRB:ApiUrl")}/{this._configurationUtility.GetValue("NBRB:CurrenciesRoute")}";

                    break;
                }
                case NationalBankAction.GetCurrency:
                {
                    url = $"{this._configurationUtility.GetValue("NBRB:ApiUrl")}/{this._configurationUtility.GetValue("NBRB:CurrencyRoute")}/{parameters.First()}?parammode=2";

                    break;
                }
            }

            return (await this._httpUtility.SendRequest<string>(HttpMethod.Get, url)).FromJson<T>();
        }
    }
}
