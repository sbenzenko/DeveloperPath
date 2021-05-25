namespace DeveloperPath.Domain.Shared.ProblemDetails
{
    public class ConflictProblemDetails : ProblemDetailsBase
    {
        public ConflictProblemDetails()
        {
            Title = "One or more errors occurred.";
            Status = 409;
        }
        public string Error;
        public string ErrorKey;
        public ConflictProblemDetails(string message, string errorKey) : this()
        {
            if (!string.IsNullOrEmpty(message))
            {
                ErrorKey = errorKey;
                Error = message;
            }
        }
    }
}
