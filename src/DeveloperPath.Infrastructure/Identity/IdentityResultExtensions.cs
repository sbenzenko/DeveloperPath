using System.Linq;
using Microsoft.AspNetCore.Identity;
using DeveloperPath.Application.Common.Models;

namespace DeveloperPath.Infrastructure.Identity
{
    public static class IdentityResultExtensions
    {
        public static Result ToApplicationResult(this IdentityResult result)
        {
            return result.Succeeded
                ? Result.Success()
                : Result.Failure(result.Errors.Select(e => e.Description));
        }
    }
}