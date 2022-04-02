using System.Collections.Generic;
using System.Threading.Tasks;
using DeveloperPath.Shared;
using DeveloperPath.Shared.ClientModels;
using DeveloperPath.WebUI.Commons;
using Microsoft.AspNetCore.JsonPatch;

namespace DeveloperPath.WebUI.Services
{
    public class PathService
    {
        private readonly HttpService _httpService;
        const string BaseResourceString = "api/paths";

        private string GetQueryString(bool onlyVisible, int pageNum, int pageSize)
            => $"?onlyVisible={onlyVisible}&pageNumber={pageNum}&pageSize={pageSize}";

        public PathService(HttpService httpService)
        {
            _httpService = httpService;
        }

        public async Task<ListWithMetadata<Path>> GetListAsync(bool onlyVisible=true, int pageNum = 1, int pageSize = 5)
        {
            return await _httpService.GetListAsync<Path>($"{BaseResourceString}{GetQueryString(onlyVisible, pageNum, pageSize)}");
        }

        public async Task<Path> AddNewPathAsync(Path path)
        {
            return await _httpService.CreateAsync(BaseResourceString, path);
        }

        public async Task<Path> ChangeVisibility(Path pathItem)
        {
            JsonPatchDocument patchDocument = new JsonPatchDocument();
            patchDocument.Replace(nameof(pathItem.Visible), !pathItem.Visible);
            return await _httpService.PatchAsync<Path>($"{BaseResourceString}/{pathItem.Id}", patchDocument);
        }

        public async Task<Path> EditPathAsync(Path path)
        {
            return await _httpService.PutAsync($"{BaseResourceString}/{path.Id}", path);
        }

        public async Task<bool> DeletePath(Path path)
        {
            return await _httpService.DeleteAsync($"{BaseResourceString}/{path.Id}");
        }

        public async Task<Path> GetAnonymousAsync(string pathKey)
        {
            return await _httpService.GetAnonymousAsync<Path>($"{BaseResourceString}/1"); //{pathKey}");
        }
        public async Task<ListWithMetadata<Path>> GetListAnonymousAsync(bool onlyVisible = true, int pageNum = 1, int pageSize = 5)
        {
            return await _httpService.GetListAnonymousAsync<Path>($"{BaseResourceString}{GetQueryString(onlyVisible, pageNum, pageSize)}");
        }

        public async Task<List<DeletedPath>> GetDeletedListAsync()
        {
            //return await _httpService.GetListAsync<DeletedPath>($"{BaseResourceString}/deleted");
            return null;
        }

        public async Task<Path> RestoreDeletedPathAsync(DeletedPath deletedPath)
        {
            JsonPatchDocument patchDocument = new JsonPatchDocument();
            patchDocument.Replace(nameof(deletedPath.Deleted), null);
            return await _httpService.PatchAsync<Path>($"{BaseResourceString}/deleted/{deletedPath.Id}", patchDocument);
        }
    }
}
