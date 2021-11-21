using System.Linq;

namespace DeveloperPath.WebApi.Models
{
    public class RequestParams
    {
        public RequestParams()
        {
            PageNumber = 1;
            PageSize = 1;
        }
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
    }
}
