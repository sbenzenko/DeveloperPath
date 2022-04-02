using DeveloperPath.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;

namespace DeveloperPath.WebApi.Helpers;

public class PagedListHeadersHelper
{
  private readonly IUrlHelper _urlHelper;
  public PagedListHeadersHelper(IUrlHelperFactory urlHelperFactory, IActionContextAccessor actionContextAccessor)
  {
    this._urlHelper = urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext);
  }
  public string CreatePathResourceUri(int currentPage, int pageSize, PathRequestParams requestParams, ResourceUriType resourceUriType)
  {
    switch (resourceUriType)
    {
      case ResourceUriType.NextPage:
        return _urlHelper.Link("GetPaths", new
        {
          pageNumber = currentPage + 1,
          pageSize = pageSize,
          onlyVisible = requestParams.OnlyVisible
        });
      case ResourceUriType.PreviousPage:
        return _urlHelper.Link("GetPaths", new
        {
          pageNumber = currentPage - 1,
          pageSize = pageSize,
          onlyVisible = requestParams.OnlyVisible
        });
      default:
        return _urlHelper.Link("GetPaths", new
        {
          pageNumber = currentPage,
          pageSize = pageSize,
          onlyVisible = requestParams.OnlyVisible
        });
    }
  }
}
