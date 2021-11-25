using DeveloperPath.Shared;
using System.Collections.Generic;

namespace DeveloperPath.WebUI.Commons
{
    public record ListWithMetadata<T>
    {
        public List<T> Data { get; init; }
        public PaginationMetadata Metadata { get; init; }
    }
}
