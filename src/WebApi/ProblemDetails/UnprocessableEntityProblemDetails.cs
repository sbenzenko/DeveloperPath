using System.Collections.Generic;

namespace DeveloperPath.WebApi.ProblemDetails
{
    public class UnprocessableEntityProblemDetails: ProblemDetailsBase
    {
        public UnprocessableEntityProblemDetails()
        {
            Title = "One or more errors occurred.";
            Status = 422;
        }

        public IDictionary<string, string> Errors = new Dictionary<string, string>();
    }
}
