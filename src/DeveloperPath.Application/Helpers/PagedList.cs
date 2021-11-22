using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DeveloperPath.Application.Helpers
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PagedList<T>: List<T>
    {
        /// <summary>
        /// Current page
        /// </summary>
        public int CurrentPage { get; }
        /// <summary>
        /// Total number of pages
        /// </summary>
        public int TotalPages { get; }
        /// <summary>
        /// Total count of entities
        /// </summary>
        public int TotalCount { get; }
        /// <summary>
        /// Current Page Size
        /// </summary>
        public int PageSize { get; }
        /// <summary>
        /// If has previous page
        /// </summary>
        public bool HasPrevious => CurrentPage > 1;
        /// <summary>
        /// If has the next page
        /// </summary>
        public bool HasNext => CurrentPage < TotalPages;

        private PagedList(IEnumerable<T> items, int count, int pageNumber, int pageSize)
        {
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalCount = count;
            TotalPages = (int)Math.Ceiling(count / (double) PageSize);
            AddRange(items);
        }

        /// <summary>
        /// Creates a list based on the portion of data
        /// </summary>
        /// <param name="source"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public static async Task<PagedList<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize, CancellationToken ct = default)
        {
            var count = await source.CountAsync(cancellationToken: ct);
            var items = await source.Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .ToListAsync(cancellationToken: ct);
            
            return new PagedList<T>(items, count, pageNumber, pageSize);
        }
    }
}
