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
    public class PagedList<T>
    {
        public List<T> Items { get; private set; }
        public int CurrentPage { get; private set; }
        public int TotalPages { get; private set;}
        public int TotalCount { get; private set;}
        public int PageSize { get; private set; }
        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;

        private PagedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            Items = items;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalCount = count;
            TotalPages = (int)Math.Ceiling(count / (double) PageSize);
        }

        public static async Task<PagedList<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize, CancellationToken ct = default)
        {
            var count = source.Count();
            var items = await source.Skip(pageSize * (pageNumber - 1))
                .Take(pageNumber)
                .ToListAsync(ct);

            return new PagedList<T>(items, count, pageNumber, pageSize);
        }
    }
}
