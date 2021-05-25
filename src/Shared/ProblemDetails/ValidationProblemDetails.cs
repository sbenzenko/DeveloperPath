namespace DeveloperPath.Domain.Shared.ProblemDetails
{
    public class ValidationProblemDetails: ProblemDetailsBase
    {
        public ValidationProblemDetails()
        {
            Title = "One or more errors occurred.";
            Status = 400;
        }
        public string Error;
        public string ErrorKey;
        public ValidationProblemDetails(string message) : this()
        {
            if (!string.IsNullOrEmpty(message))
            {
                ErrorKey = "INVALID_INPUT_JSON";
                Error = message;
            }
        }
    }
}
