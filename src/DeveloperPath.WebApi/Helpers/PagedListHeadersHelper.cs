using DeveloperPath.WebApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace DeveloperPath.WebApi.Helpers
{
    public class PagedListHeadersHelper
    {
        private readonly IUrlHelper _url;

        public PagedListHeadersHelper(IUrlHelper urlHelper)
        {
            this._url = urlHelper;
        }
        public string CreatePathResourceUri(int currentPage, int pageSize, PathRequestParams requestParams, ResourceUriType resourceUriType)
        {
            switch (resourceUriType)
            {
                case ResourceUriType.NextPage:
                    return _url.Link("GetPaths", new
                    {
                        pageNumber = currentPage + 1,
                        pageSize = pageSize,
                        onlyVisible = requestParams.OnlyVisible
                    });
                case ResourceUriType.PreviousPage:
                    return _url.Link("GetPaths", new
                    {
                        pageNumber = currentPage - 1,
                        pageSize = pageSize,
                        onlyVisible = requestParams.OnlyVisible
                    });
                default:
                    return _url.Link("GetPaths", new
                    {
                        pageNumber = currentPage,
                        pageSize = pageSize,
                        onlyVisible = requestParams.OnlyVisible
                    });
            }
        }
    }
}
