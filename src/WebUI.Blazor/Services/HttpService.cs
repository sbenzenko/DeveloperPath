using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace WebUI.Blazor.Services
{
    public class HttpService
    {
        public HttpClient HttpClient { get; }
        public HttpService(HttpClient httpClient)
        {
            HttpClient = httpClient;
        }

        public async Task<List<T>> GetListAsync<T>(string resourceUri)
        {
            var responce = await HttpClient.GetStreamAsync(resourceUri);
            return await JsonSerializer.DeserializeAsync<List<T>>(responce, new JsonSerializerOptions() {PropertyNameCaseInsensitive = true});
        }
    }
}
