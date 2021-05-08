using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
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
            var response = await HttpClient.GetStreamAsync(resourceUri);
            return await JsonSerializer.DeserializeAsync<List<T>>(response, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }

        public async Task<T> CreateAsync<T>(string baseResourceString, T resource)
        {
            var response = await HttpClient.PostAsJsonAsync(baseResourceString, new{});

            if (response.IsSuccessStatusCode)
            {
                var result = await JsonSerializer.DeserializeAsync<T>(await response.Content.ReadAsStreamAsync(),
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return result;
            }

            if (response.StatusCode == HttpStatusCode.UnprocessableEntity)
            {
                var unprocessableResult = await JsonSerializer.DeserializeAsync<UnprocessableEntityObjectResult>(
                    await response.Content.ReadAsStreamAsync(),
                    new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            }
            if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                throw new Exception("Server returned error");
            }
            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                throw new Exception("Server returned Bad Request error");
            }
            throw new Exception("Server returned error");
        }
    }

    public class UnprocessableEntityObjectResult
    {
    }
}
