using DeveloperPath.Application.Common.Models;
using System.Threading.Tasks;

namespace DeveloperPath.Application.Common.Interfaces
{
  /// <summary>
  /// Identity service interface
  /// </summary>
  public interface IIdentityService
  {
    /// <summary>
    /// Get user name
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<string> GetUserNameAsync(string userId);

    /// <summary>
    /// Create user
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    Task<(Result Result, string UserId)> CreateUserAsync(string userName, string password);

    /// <summary>
    /// Delete user
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<Result> DeleteUserAsync(string userId);
  }
}
