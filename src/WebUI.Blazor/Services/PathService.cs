using System.Collections.Generic;
using System.Threading.Tasks;
using DeveloperPath.Domain.Shared.ClientModels;
using Microsoft.AspNetCore.JsonPatch;
using Newtonsoft.Json.Serialization;

namespace WebUI.Blazor.Services
{
    public class PathService
    {
        private readonly HttpService _httpService;
        const string BaseResourceString = "api/paths";


        public PathService(HttpService httpService)
        {
            _httpService = httpService;
        }

        public async Task<List<Path>> GetListAsync()
        {
            return await _httpService.GetListAsync<Path>(BaseResourceString);
        }

        public async Task<Path> AddNewPathAsync(Path path)
        {
            return await _httpService.CreateAsync(BaseResourceString, path);
        }

        public async Task<Path> ChangeVisibility(Path pathItem)
        {
            JsonPatchDocument  patchDocument = new JsonPatchDocument();
            patchDocument.Replace(nameof(pathItem.IsVisible), !pathItem.IsVisible);
           
            return await _httpService.PatchAsync<Path>($"{BaseResourceString}/{pathItem.Id}", patchDocument);
        }
    }
}
