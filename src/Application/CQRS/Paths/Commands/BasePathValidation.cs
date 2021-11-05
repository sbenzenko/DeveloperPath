using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DeveloperPath.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DeveloperPath.Application.CQRS.Paths.Commands
{
    /// <summary>
    ///  Base validation logic interface for path related to data base checking
    /// </summary>
    public interface IBasePathValidation
    {
        /// <summary>
        /// Request to context to check for unique URI key
        /// </summary>
        /// <param name="id">Path Id</param>
        /// <param name="key"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> BeUniqueKey(int? id, string key, CancellationToken cancellationToken);

        /// <summary>
        /// Request to context to check for unique title
        /// </summary>
        /// <param name="id">Path Id</param>
        /// <param name="title"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> BeUniqueTitle(int? id, string title, CancellationToken cancellationToken);
    }

    /// <summary>
    /// Base validation logic for path related to data base checking
    /// </summary>
    public class BasePathValidation : IBasePathValidation
    {
        private readonly IApplicationDbContext _context;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public BasePathValidation(IApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Request to context to check for unique URI key
        /// </summary>
        /// <param name="id">Path Id</param>
        /// <param name="key"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> BeUniqueKey(int? id, string key, CancellationToken cancellationToken)
        {
            var query = _context.Paths.AsQueryable();
            if (id != default)
                query = query.Where(p => p.Id != id);
            return await query.AllAsync(l => l.Key != key, cancellationToken);
        }

        /// <summary>
        /// Request to context to check for unique title
        /// </summary>
        /// <param name="id">Path Id</param>
        /// <param name="title"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> BeUniqueTitle(int? id, string title, CancellationToken cancellationToken)
        {
            var query = _context.Paths.AsQueryable();
            if (id != default)
                query = query.Where(p => p.Id != id);
            return await query.AllAsync(l => l.Title != title, cancellationToken);
        }
    }
}
