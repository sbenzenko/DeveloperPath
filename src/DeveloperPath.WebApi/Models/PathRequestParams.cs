namespace DeveloperPath.WebApi.Models;

public class PathRequestParams : RequestParams
{
  public bool OnlyVisible { get; init; } = true;

  public PathRequestParams()
  { }

  public PathRequestParams(PathRequestParams requestParams)
      : base(requestParams)
  {
    if (requestParams is null) return;
    OnlyVisible = requestParams.OnlyVisible;
  }
}