namespace DeveloperPath.Domain.Shared.ProblemDetails
{
    public class NotFoundProblemDetails : ProblemDetailsBase
    {
        public NotFoundProblemDetails()
        {
            Title = "One or more errors occurred.";
            Status = 404;
        }

        public string Error;
        public string ErrorKey;

        public NotFoundProblemDetails(string message, string errorKey) : this()
        {
            ErrorKey = errorKey;
            Error = message;
        }
    }
}
