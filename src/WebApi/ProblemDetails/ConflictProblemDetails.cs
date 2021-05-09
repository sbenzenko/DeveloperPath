namespace DeveloperPath.WebApi.ProblemDetails
{
    public class ConflictProblemDetailsBase : ProblemDetails.ProblemDetailsBase
    {
        public ConflictProblemDetailsBase()
        {
            Title = "One or more errors occurred.";
            Status = 409;
        }
        public string Error;
        public string ErrorKey;
        public ConflictProblemDetailsBase(string message, string errorKey) : this()
        {
            if (!string.IsNullOrEmpty(message))
            {
                ErrorKey = errorKey;
                Error = message;
            }
        }
    }
}
