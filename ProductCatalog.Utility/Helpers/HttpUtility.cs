using ProductCatalog.Utility.Extensions;
using System.Text;

namespace ProductCatalog.Utility.Helpers
{
    public class HttpUtility
    {
        public async Task<string> SendRequest<T>(HttpMethod httpMethod, string url, T model = null) where T : class
        {
            using var client = new HttpClient();
            using var requestMessage = new HttpRequestMessage(httpMethod, url);
            if (model != null)
            {
                var stringContent = new StringContent(model.ToJson(), Encoding.UTF8, "application/json");
                requestMessage.Content = stringContent;
            }

            using (requestMessage.Content)
            {
                using var response = await client.SendAsync(requestMessage);
                using var content = response.Content;
                var responseBody = await content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Error with a {httpMethod} request to {url}. StatusCode: {response.StatusCode}. ReasonPhrase: {response.ReasonPhrase}. ResponseBody: {responseBody}");
                }
                return responseBody;
            }
        }
    }
}
