using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Shared.ProblemDetails
{
    public class UnprocessableEntityProblemDetails: ProblemDetailsBase
    {
        public UnprocessableEntityProblemDetails()
        {
            Title = "One or more validation errors occurred.";
            Status = 422;
        }

        /// <summary>
        /// List of errors
        /// </summary>
        [JsonPropertyName("errors")]
        public IDictionary<string, string[]> Errors { get; set; } = new Dictionary<string, string[]>();
    }
}
