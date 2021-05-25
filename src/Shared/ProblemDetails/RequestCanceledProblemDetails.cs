namespace DeveloperPath.Domain.Shared.ProblemDetails
{
    public class RequestCanceledProblemDetails : ProblemDetailsBase
    {
        public RequestCanceledProblemDetails()
        {
            Title = "The request was canceled by calling side.";
            Status = 499;
        }

        public string Error;
        public RequestCanceledProblemDetails(string message) : this()
        {
            if (!string.IsNullOrEmpty(message))
            {
                Error = message;
            }
        }
    }
}
