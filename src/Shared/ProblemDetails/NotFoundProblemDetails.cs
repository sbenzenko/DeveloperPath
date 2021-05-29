using DeveloperPath.Domain.Shared.ProblemDetails;

namespace Shared.ProblemDetails
{
    public class NotFoundProblemDetails : ProblemDetailsBase
    {
        public NotFoundProblemDetails()
        {
            Title = "One or more errors occurred.";
            Status = 404;
        }

        public string Error { get; set; }
        public string ErrorKey { get; set; }

        public NotFoundProblemDetails(string message, string errorKey) : this()
        {
            ErrorKey = errorKey;
            Error = message;
        }
    }
}
