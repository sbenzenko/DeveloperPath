using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Shared.Dtos.Models;

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

        public async Task<PathDto[]> GetListAsync()
        {
            return await _httpService.GetListAsync<PathDto>(BaseResourceString);
        }
    }
}
