namespace Shared.ProblemDetails
{
    public class NotFoundProblemDetailsBase : ProblemDetailsBase
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
