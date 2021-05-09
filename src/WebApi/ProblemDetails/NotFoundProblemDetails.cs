namespace DeveloperPath.WebApi.ProblemDetails
{
    public class NotFoundProblemDetailsBase : ProblemDetails.ProblemDetailsBase
    {
        public NotFoundProblemDetailsBase()
        {
            Title = "One or more errors occurred.";
            Status = 404;
        }

        public string Error;
        public string ErrorKey;

        public NotFoundProblemDetailsBase(string message, string errorKey) : this()
        {
            ErrorKey = errorKey;
            Error = message;
        }
    }
}
