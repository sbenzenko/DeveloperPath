using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeveloperPath.WebApi
{
    public record RequestParams
    {
        public int PageNumber { get; init; }
        public int PageSize { get; init; }
    }
}
