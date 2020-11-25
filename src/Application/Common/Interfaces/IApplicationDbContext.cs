using DeveloperPath.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace DeveloperPath.Application.Common.Interfaces
{
  //TODO: update with DeveloperPath entities
    public interface IApplicationDbContext
    {

        //TODO: this is from the template. Remove
        DbSet<TodoList> TodoLists { get; set; }

        //TODO: this is from the template. Remove
        DbSet<TodoItem> TodoItems { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
