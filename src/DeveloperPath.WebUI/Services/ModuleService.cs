using System.Collections.Generic;
using System.Threading.Tasks;
using DeveloperPath.Shared.ClientModels;

namespace DeveloperPath.WebUI.Services
{
    public class ModuleService
    {
        private readonly HttpService _httpService;
        const string BaseResourceString = "api/modules";


        public ModuleService(HttpService httpService)
        {
            _httpService = httpService;
        }

        //public async Task<List<Module>> GetListAsync(string pathKey)
        //{
        //    return await _httpService.GetListAsync<Module>(BaseResourceString(pathKey));
        //}

        public async Task<List<Module>> GetListAsync()
        {
            //return await _httpService.GetListAsync<Module>(BaseResourceString);
            return null;
        }
        public async Task<Module> EditModuleAsync(Module module)
        {
            return await _httpService.PutAsync($"{BaseResourceString}/{module.Id}", module);
        }

        //public async Task<Path> AddNewPathAsync(Path path)
        //{
        //    return await _httpService.CreateAsync(BaseResourceString, path);
        //}

        //public async Task<Path> ChangeVisibility(Path pathItem)
        //{
        //    JsonPatchDocument patchDocument = new JsonPatchDocument();
        //    patchDocument.Replace(nameof(pathItem.IsVisible), !pathItem.IsVisible);
        //    return await _httpService.PatchAsync<Path>($"{BaseResourceString}/{pathItem.Id}", patchDocument);
        //}



        //public async Task<bool> DeletePath(Path path)
        //{
        //    return await _httpService.DeleteAsync($"{BaseResourceString}/{path.Id}");
        //}

        //public async Task<List<Path>> GetListAnonymousAsync()
        //{
        //    return await _httpService.GetListAnonymousAsync<Path>(BaseResourceString);
        //}

        //public async Task<List<DeletedPath>> GetDeletedListAsync()
        //{
        //    return await _httpService.GetListAsync<DeletedPath>($"{BaseResourceString}/deleted");
        //}

        //public async Task<Path> RestoreDeletedPathAsync(DeletedPath deletedPath)
        //{
        //    JsonPatchDocument patchDocument = new JsonPatchDocument();
        //    patchDocument.Replace(nameof(deletedPath.Deleted), null);
        //    return await _httpService.PatchAsync<Path>($"{BaseResourceString}/deleted/{deletedPath.Id}", patchDocument);
        //}

    }
}
