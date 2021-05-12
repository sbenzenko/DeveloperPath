using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using DeveloperPath.Domain.Shared.ClientModels;
using Microsoft.AspNetCore.JsonPatch;
using Newtonsoft.Json;
using Shared.ProblemDetails;
using JsonSerializer = System.Text.Json.JsonSerializer;

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
            var response = await HttpClient.PostAsJsonAsync(baseResourceString, resource);

            if (response.IsSuccessStatusCode)
            {
                var result = await JsonSerializer.DeserializeAsync<T>(await response.Content.ReadAsStreamAsync(),
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return result;
            }

            if (response.StatusCode == HttpStatusCode.UnprocessableEntity)
            {
                var unprocessableResult = await JsonSerializer.DeserializeAsync<UnprocessableEntityProblemDetails>(
                    await response.Content.ReadAsStreamAsync(),
                    new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                throw new ApiError(unprocessableResult);
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

        public async Task<TModel> PatchAsync<TModel>(string resourceUri, JsonPatchDocument patchDocument)
        {
            var serializedItemToUpdate = JsonConvert.SerializeObject(patchDocument);
            var response = await HttpClient.PatchAsync(resourceUri, new StringContent(serializedItemToUpdate, null, "application/json"));
            if (response.IsSuccessStatusCode)
            {
                var result = await JsonSerializer.DeserializeAsync<TModel>(await response.Content.ReadAsStreamAsync(),
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return result;
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
}
