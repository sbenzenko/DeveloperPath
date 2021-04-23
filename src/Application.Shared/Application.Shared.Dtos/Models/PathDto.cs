using System.Collections.Generic;

namespace Application.Shared.Dtos.Models
{
    /// <summary>
    /// Developer path information
    /// </summary>
    public class PathDto
    {
        /// <summary>
        /// Path ID
        /// </summary>
        public int Id { get; init; }

        /// <summary>
        /// Path title
        /// </summary>
        public string Title { get; init; }

        /// <summary>
        /// Path short summary
        /// </summary>
        public string Description { get; init; }

        /// <summary>
        /// List of tags related to path
        /// </summary>
        public ICollection<string> Tags { get; set; }
    }
}
