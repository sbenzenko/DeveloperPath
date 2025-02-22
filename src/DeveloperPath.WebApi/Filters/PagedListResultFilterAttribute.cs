﻿using System.Linq;
using System.Text.Json;

using DeveloperPath.Application.Helpers;
using DeveloperPath.Shared;
using DeveloperPath.Shared.ClientModels;
using DeveloperPath.WebApi.Helpers;
using DeveloperPath.WebApi.Models;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace DeveloperPath.WebApi.Filters;

public class PagedListResultFilterAttribute : IActionFilter
{
  PathRequestParams _requestParams;
  readonly JsonSerializerOptions _options = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

  public void OnActionExecuted(ActionExecutedContext context)
  {
    if (context.Result is OkObjectResult okResult && okResult.Value is PagedList<Path> pagedList)
    {
      var helper = context.HttpContext.RequestServices.GetService<PagedListHeadersHelper>();

      var nextPageLink = pagedList.HasNext ?
          helper.CreatePathResourceUri(pagedList.CurrentPage, pagedList.PageSize, _requestParams, ResourceUriType.NextPage)
          : null;
      var prevPageLink = pagedList.HasPrevious ?
          helper.CreatePathResourceUri(pagedList.CurrentPage, pagedList.PageSize, _requestParams, ResourceUriType.PreviousPage)
          : null;

      var paginationMetadata = new PaginationMetadata
      {
        TotalCount = pagedList.TotalCount,
        PageSize = pagedList.PageSize,
        CurrentPage = pagedList.CurrentPage,
        TotalPages = pagedList.TotalPages,
        PrevPageLink = prevPageLink,
        NextPageLink = nextPageLink
      };

      context.HttpContext.Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(paginationMetadata, _options));
    }
  }

  public void OnActionExecuting(ActionExecutingContext context)
  {
    var param = context.ActionArguments.SingleOrDefault(p => p.Value is PathRequestParams);
    if (param.Value is PathRequestParams reqParams)
      _requestParams = reqParams;
  }
}
