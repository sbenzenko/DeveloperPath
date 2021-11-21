using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DeveloperPath.Application.Common.Models;
using DeveloperPath.Application.Helpers;
using Microsoft.EntityFrameworkCore;

namespace DeveloperPath.Application.Extensions
{
    /// <summary>
    /// Encapsulates pagination logic
    /// </summary>
    public static class PagedDataExtension
    {
        /// <summary>
        /// Get portion of data
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static Task<PagedList<T>> ToPagedListAsync<T>(this IQueryable<T> query, int pageSize, int pageNumber,
            CancellationToken cancellationToken = default)
            => PagedList<T>.CreateAsync(query, pageNumber, pageSize, cancellationToken);
    }
}
