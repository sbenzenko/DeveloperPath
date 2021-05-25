using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using DeveloperPath.Domain.Shared.ClientModels;

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

        public async Task<Path> EditPathAsync(Path path)
        {
            return await _httpService.PutAsync<Path>($"{BaseResourceString}/{path.Id}", path);
        }

        public async Task<bool> DeletePath(Path path)
        {
            return await _httpService.DeleteAsync($"{BaseResourceString}/{path.Id}");
        }

        public async Task<List<Path>> GetListAnonymousAsync()
        {
            return  await _httpService.GetListAnonymousAsync<Path>(BaseResourceString);
        }

        public async Task<List<DeletedPath>> GetDeletedListAsync()
        {
            return await _httpService.GetListAsync<DeletedPath>($"{BaseResourceString}/deleted");
        }

        public async Task<Path> RestoreDeletedPathAsync(DeletedPath deletedPath)
        {
            JsonPatchDocument patchDocument = new JsonPatchDocument();
            patchDocument.Replace(nameof(deletedPath.Deleted), null);
            return await _httpService.PatchAsync<Path>($"{BaseResourceString}/deleted/{deletedPath.Id}", patchDocument);
        }
    }
}
