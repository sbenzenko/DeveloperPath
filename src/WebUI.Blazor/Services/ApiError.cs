using System;
using DeveloperPath.Domain.Shared.ProblemDetails;

namespace WebUI.Blazor.Services
{
    internal class ApiError : Exception
    {
        public ProblemDetailsBase ProblemDetails { get; }

        public ApiError()
        {
        }

        public ApiError(ProblemDetailsBase problemDetails)
        {
            ProblemDetails = problemDetails;
        }
    }
}