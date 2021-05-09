using System.Collections.Generic;

namespace DeveloperPath.WebApi.ProblemDetails
{
    public class UnprocessableEntityProblemDetailsBase: ProblemDetails.ProblemDetailsBase
    {
        public UnprocessableEntityProblemDetailsBase()
        {
            Title = "One or more errors occurred.";
            Status = 422;
        }

        public IDictionary<string, string[]> Errors = new Dictionary<string, string[]>();
    }
}
