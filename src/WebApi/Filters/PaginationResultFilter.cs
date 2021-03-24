using System;
using System.Threading.Tasks;
using DeveloperPath.Application.Paging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DeveloperPath.WebApi.Filters
{
    public class PaginationResultFilter : Attribute, IAsyncResultFilter
    {
        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            //TODO: 
            // 1. Result should have both pagination object and request results
            var result = context.Result as ObjectResult;
            var page = result.Value;
            var type = page.GetType();

            Type genericType = default;
            if (type.IsGenericType)
            {
                genericType = type.GenericTypeArguments[0];
            }

            if (genericType != null && type.IsAssignableFrom(typeof(PagedList<>).MakeGenericType(genericType)))
            {
                // 3. Send pagination object to response header
                var paginationData = type.GetProperty("Pagination").GetValue(page);
                context.HttpContext.Response.Headers.Add("X-Pagination",
                    System.Text.Json.JsonSerializer.Serialize(paginationData));
            }



            // 4. Send results to Response.Body

            await next();
        }
    }
}