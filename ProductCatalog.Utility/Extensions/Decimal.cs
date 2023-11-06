namespace ProductCatalog.Utility.Extensions
{
    public static class Decimal
    {
        public static string ConvertToCurrencyString(this decimal value, decimal rate, string currencyAbbreviation)
        {
            var convertedValue = Math.Round(value / rate, 2);

            return $"{convertedValue} {currencyAbbreviation}";
        }
    }
}
