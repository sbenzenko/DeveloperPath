using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

using DeveloperPath.Shared;
using DeveloperPath.Shared.ProblemDetails;
using DeveloperPath.WebUI.Commons;

using Microsoft.AspNetCore.JsonPatch;

using Newtonsoft.Json;

using JsonSerializer = System.Text.Json.JsonSerializer;

namespace DeveloperPath.WebUI.Services;

public class HttpService(IHttpClientFactory clientFactory)
{
  public HttpClient HttpClient { get; } = clientFactory.CreateClient("api");
  public HttpClient AnonymousHttpClient { get; } = clientFactory.CreateClient("api-anonymous");
  private readonly JsonSerializerOptions _deserializeOptions = new() { PropertyNameCaseInsensitive = true };

  public async Task<ListWithMetadata<T>> GetListAsync<T>(string resourceUri)
  {
    var response = await HttpClient.GetAsync(resourceUri);
    var stream = await response.Content.ReadAsStreamAsync();

    if (response.StatusCode == HttpStatusCode.NotFound)
    {
      var notFound = await JsonSerializer.DeserializeAsync<NotFoundProblemDetails>(stream, _deserializeOptions);
      throw new ApiError(notFound, HttpStatusCode.NotFound);
    }

    var result = await JsonSerializer.DeserializeAsync<List<T>>(stream, _deserializeOptions);
    if (response.Headers.TryGetValues("x-pagination", out var values))
    {
      var paginationMeta = JsonSerializer.Deserialize<PaginationMetadata>(values.First(), _deserializeOptions);
      return new ListWithMetadata<T>
      {
        Data = result,
        Metadata = paginationMeta
      };
    }

    return new ListWithMetadata<T>
    {
      Data = result,
      Metadata = new PaginationMetadata()
    };
  }

  public async Task<T> CreateAsync<T>(string resourceUri, T resource)
  {
    var response = await HttpClient.PostAsJsonAsync(resourceUri, resource);

    if (response.IsSuccessStatusCode)
    {
      var result = await JsonSerializer.DeserializeAsync<T>(await response.Content.ReadAsStreamAsync(), _deserializeOptions);
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
      var result = await JsonSerializer.DeserializeAsync<TModel>(await response.Content.ReadAsStreamAsync(), _deserializeOptions);
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
      var result = await JsonSerializer.DeserializeAsync<T>(await response.Content.ReadAsStreamAsync(), _deserializeOptions);
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
          await response.Content.ReadAsStreamAsync(), _deserializeOptions);
      throw new ApiError(unprocessableResult, HttpStatusCode.UnprocessableEntity);
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

  public async Task<ListWithMetadata<T>> GetListAnonymousAsync<T>(string resourceUri)
  {

    var response = await AnonymousHttpClient.GetAsync(resourceUri);
    var result = await JsonSerializer.DeserializeAsync<List<T>>(await response.Content.ReadAsStreamAsync(), _deserializeOptions);

    if (response.Headers.TryGetValues("x-pagination", out var values))
    {
      var paginationMeta = JsonSerializer.Deserialize<PaginationMetadata>(values.First(), _deserializeOptions);
      return new ListWithMetadata<T>
      {
        Data = result,
        Metadata = paginationMeta
      };
    }

    return new ListWithMetadata<T>
    {
      Data = result,
      Metadata = new PaginationMetadata()
    };
  }
}
