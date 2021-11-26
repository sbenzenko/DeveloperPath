using System;
using System.Net;
using DeveloperPath.Shared.ProblemDetails;

namespace DeveloperPath.WebUI.Services
{
    internal class ApiError : Exception
    {
        public ProblemDetailsBase ProblemDetails { get; }
        public HttpStatusCode StatusCode { get; }

        public ApiError(ProblemDetailsBase problemDetails, HttpStatusCode statusCode)
        {
            ProblemDetails = problemDetails;
            StatusCode = statusCode;
        }
    }
}