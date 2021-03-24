using System;

namespace DeveloperPath.Application.Paging
{
    public class PaginationData
    {
        public int PageNumber { get;  }
        public int PageSize { get;  }
        public Uri FirstPage { get; }
        public Uri LastPage { get; }
        public int TotalPages => (int)Math.Ceiling(TotalRecords / (decimal)PageSize);
        public int TotalRecords { get;} 
        public Uri NextPage { get;  }
        public Uri PreviousPage { get; }

        public PaginationData(int pageNumber, int pageSize, int totalRecords)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalRecords = totalRecords;

            if(pageNumber > TotalPages)
                throw new ArgumentException("The page number should be lower than total pages number");
        }
    }
}
