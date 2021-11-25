namespace DeveloperPath.WebApi.Helpers
{
    public static class PagedListHeadersHelper
    {
        public static string CreatePathResourceUri(int currentPage, int pageSize, ResourceUriType resourceUriType)
        {
            switch (resourceUriType)
            {
                case ResourceUriType.NextPage:
                    return _url.Link("GetPaths", new
                    {
                        pageNumber = currentPage + 1,
                        pageSize = pageSize,
                        onlyVisible = _requestParams.OnlyVisible
                    });
                case ResourceUriType.PreviousPage:
                    return _url.Link("GetPaths", new
                    {
                        pageNumber = currentPage - 1,
                        pageSize = pageSize,
                        onlyVisible = _requestParams.OnlyVisible
                    });
                default:
                    return _url.Link("GetPaths", new
                    {
                        pageNumber = currentPage,
                        pageSize = pageSize,
                        onlyVisible = _requestParams.OnlyVisible
                    });
            }
        }
    }
}
