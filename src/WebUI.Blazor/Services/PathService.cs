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

        public async Task<Path[]> GetListAsync()
        {
            return await _httpService.GetListAsync<Path>(BaseResourceString);
        }
    }
}
