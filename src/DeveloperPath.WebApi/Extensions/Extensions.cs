using DeveloperPath.WebApi.Models;

namespace DeveloperPath.WebApi.Extensions
{
  public static class Extensions
  {
    public static bool UsePaging(this RequestParams requestParams)
    {
      return requestParams.PageSize > 0 && requestParams.PageNumber > 0;
    }
  }
}
