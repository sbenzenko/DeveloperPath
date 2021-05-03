using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace DeveloperPath.WebApi.Extensions
{
  [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "This is a conventions class")]
  internal static class ApiCustomConventions
  {
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesDefaultResponseType]
    public static void Get(
      [ApiConventionTypeMatch(ApiConventionTypeMatchBehavior.Any)]
      object requestParams)
    { }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public static void Get(
      [ApiConventionNameMatch(ApiConventionNameMatchBehavior.Any)]
      int id,
      [ApiConventionTypeMatch(ApiConventionTypeMatchBehavior.Any)]
      params object[] requestParams)
    { }

    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status406NotAcceptable)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status415UnsupportedMediaType)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
    [ProducesDefaultResponseType]
    public static void Create(
      [ApiConventionTypeMatch(ApiConventionTypeMatchBehavior.Any)]
      params object[] requestParams)
    { }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status406NotAcceptable)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status415UnsupportedMediaType)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
    [ProducesDefaultResponseType]
    public static void Update(
      [ApiConventionNameMatch(ApiConventionNameMatchBehavior.Any)]
      int id,
      [ApiConventionTypeMatch(ApiConventionTypeMatchBehavior.Any)]
      params object[] requestParams)
    { }

    [ProducesResponseType(typeof(NoContentResult), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public static void Delete(
      [ApiConventionTypeMatch(ApiConventionTypeMatchBehavior.Any)]
      params object[] requestParams)
    { }
  }
}
