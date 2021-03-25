//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Filters;

//namespace DeveloperPath.WebApi.Filters
//{
//  public class PaginationResultFilter : IAsyncResultFilter
//  {
//    public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
//    {
//      //TODO: 
//      // 1. Result should have both pagination object and request results
//      var result = context.Result as ObjectResult;
//      var page = result.Value;

//      // 2. Here split pagination object and results

//      // 3. Send pagination object to response header
//      context.HttpContext.Response.Headers.Add("X-Pagination",
//        System.Text.Json.JsonSerializer.Serialize(page));

//      // 4. Send results to Response.Body

//      await next();
//    }
//  }
//}