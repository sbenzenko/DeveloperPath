using System.Threading.Tasks;

using DeveloperPath.Shared.ClientModels;
using DeveloperPath.WebUI.Commons;

namespace DeveloperPath.WebUI.Services.Common;

public class PathService(HttpService httpService)
{
  private readonly HttpService _httpService = httpService;
  const string BaseResourceString = "api/paths";
  private static string GetQueryString(bool onlyVisible, int pageNum, int pageSize)
     => $"?onlyVisible={onlyVisible}&pageNumber={pageNum}&pageSize={pageSize}";

  public async Task<PathDetails> GetPathAsync(int id)
  {
    return await _httpService.GetAnonymousAsync<PathDetails>($"{BaseResourceString}/{id}");
  }

  public async Task<ListWithMetadata<Path>> GetListAsync(bool onlyVisible = true, int pageNum = 1, int pageSize = 5)
  {
    return await _httpService.GetListAnonymousAsync<Path>($"{BaseResourceString}{GetQueryString(onlyVisible, pageNum, pageSize)}");
  }
}