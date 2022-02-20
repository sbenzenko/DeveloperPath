using System.Linq;

namespace DeveloperPath.WebApi.Models;

public class RequestParams
{
    private int _pageSize;
    public int PageNumber { get; set; }

    public int PageSize
    {
        get => _pageSize;
        set
        {
            var max = Shared.Enums.PageSize.PageSizes.Max;
            _pageSize = value >= max ? max : Shared.Enums.PageSize.PageSizes.FirstOrDefault(x => x >= value);
        }
    }
        
    public RequestParams()
    {
        PageNumber = 1;
        PageSize = 1;
    }

    public RequestParams(RequestParams requestParams)
    {
        if (requestParams is null) return;
            
        PageNumber = requestParams.PageNumber;
        PageSize = requestParams.PageSize;
    }
}