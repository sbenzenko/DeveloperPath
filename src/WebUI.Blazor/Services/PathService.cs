using System.Collections.Generic;
using System.Threading.Tasks;
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
    }
}
