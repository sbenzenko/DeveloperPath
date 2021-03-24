using System.Collections.Generic;
using System.Linq;

namespace DeveloperPath.Application.Paging
{
    public class PagedList<T>
    {
        public PagedList(IEnumerable<T> data)
        {
            Data = data;
            Pagination = new PaginationData(1, 10, data.Count());
        }
        public PagedList(IEnumerable<T> data, PaginationData paginationData) : this(data)
        {
            Pagination = paginationData;
        }

        public IEnumerable<T> Data { get; }
        public PaginationData Pagination { get;  }
    }
}