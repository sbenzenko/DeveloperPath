using System;
using DeveloperPath.Shared.ProblemDetails;

namespace DeveloperPath.WebUI.Services
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