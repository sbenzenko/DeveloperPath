using Shared.ProblemDetails;
using System;

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