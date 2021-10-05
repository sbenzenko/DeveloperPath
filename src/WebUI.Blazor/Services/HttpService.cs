using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using Newtonsoft.Json;
using DeveloperPath.Domain.Shared.ProblemDetails;
using Shared.ProblemDetails;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace WebUI.Blazor.Services
{
    public class HttpService
    {
        private readonly IHttpClientFactory _clientFactory;
        public HttpClient HttpClient { get; }
        public HttpClient AnonymousHttpClient { get; }

        public HttpService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
            HttpClient = _clientFactory.CreateClient("api");
            AnonymousHttpClient = _clientFactory.CreateClient("api-anonymous");
        }

        public async Task<List<T>> GetListAsync<T>(string resourceUri)
        {
            var response = await HttpClient.GetAsync(resourceUri);
            var stream = await response.Content.ReadAsStreamAsync();
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                var notFound = await JsonSerializer.DeserializeAsync<NotFoundProblemDetails>(stream, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                throw new ApiError(notFound);
            }
            return await JsonSerializer.DeserializeAsync<List<T>>(stream, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }

        public async Task<T> CreateAsync<T>(string resourceUri, T resource)
        {
            var response = await HttpClient.PostAsJsonAsync(resourceUri, resource);

            if (response.IsSuccessStatusCode)
            {
                var result = await JsonSerializer.DeserializeAsync<T>(await response.Content.ReadAsStreamAsync(),
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return result;
            }

            await ThrowException(response);
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

            await ThrowException(response);
            throw new Exception("Server returned error");
        }

        public async Task<T> PutAsync<T>(string resourceUri, T resource)
        {
            var serializedItemToUpdate = JsonConvert.SerializeObject(resource);
            var response = await HttpClient.PutAsync(resourceUri,
                new StringContent(serializedItemToUpdate, null, "application/json"), CancellationToken.None);
            if (response.IsSuccessStatusCode)
            {
                var result = await JsonSerializer.DeserializeAsync<T>(await response.Content.ReadAsStreamAsync(),
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return result;
            }
            await ThrowException(response);
            throw new Exception("Server returned error");
        }

        private async Task ThrowException(HttpResponseMessage response)
        {
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
        }

        public async Task<bool> DeleteAsync(string resourceUri)
        {
            var response = await HttpClient.DeleteAsync(resourceUri);
            if (response.IsSuccessStatusCode)
                return true;
            await ThrowException(response);
            throw new Exception("Server returned error");
        }

        public async Task<List<T>> GetListAnonymousAsync<T>(string resourceUri)
        {
            var response = await AnonymousHttpClient.GetStreamAsync(resourceUri);
            return await JsonSerializer.DeserializeAsync<List<T>>(response, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }
    }
}
