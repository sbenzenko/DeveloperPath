namespace DeveloperPath.WebApi.Models
{
    public class PathRequestParams: RequestParams
    {
        public bool OnlyVisible { get; init; } = true;
    }
}
