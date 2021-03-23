using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http.Filters;
using DeveloperPath.Application.Paging;
using Microsoft.AspNetCore.Http;

namespace DeveloperPath.WebApi.Filters
{
    public class PaginationHeadersFilter : FilterAttribute, IResultFilter
    {
        private readonly PaginationData _paginationData;

        public PaginationHeadersFilter(PaginationData paginationData)
        {
            _paginationData = paginationData;
        }

        public void OnResultExecuting(ResultExecutingContext context)
        {
            context.HttpContext.Response.Headers.Add("X-Pagination", System.Text.Json.JsonSerializer.Serialize(_paginationData));
        }

        public void OnResultExecuted(ResultExecutedContext context)
        {
            // nothing
        }

        
    }
}
