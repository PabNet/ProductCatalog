using Newtonsoft.Json;

namespace ProductCatalog.Utility.Extensions
{
    public static class Json
    {
        public static string ToJson(this object model)
        {
            return JsonConvert.SerializeObject(model);
        }

        public static T FromJson<T>(this string json)
        {
            return JsonConvert.DeserializeObject<T>(json)!;
        }
    }
}
