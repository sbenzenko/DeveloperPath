namespace DeveloperPath.WebApi.Models
{
  public record RequestParams
  {
    public int PageNumber { get; init; }
    public int PageSize { get; init; }
  }
}
