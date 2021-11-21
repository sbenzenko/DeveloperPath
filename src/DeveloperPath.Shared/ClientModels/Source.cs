using System.Collections.Generic;
using DeveloperPath.Shared.Enums;

namespace DeveloperPath.Shared.ClientModels
{
    /// <summary>
    /// Source of information (book, article, blog post, course)
    /// </summary>
    public class Source
    {
        /// <summary>
        /// Source ID
        /// </summary>
        public int Id { get; init; }

        /// <summary>
        /// Source Title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Souce short summary
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Souce URL
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Theme id that the source is for
        /// </summary>
        public int ThemeId { get; set; }

        /// <summary>
        /// Theme that the source is for
        /// </summary>
        public Theme Theme { get; init; }

        /// <summary>
        /// Position of source in theme (0-based).
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// Type of source (None | Book | Blog | Course | Documentation | QandA | Video)
        /// </summary>
        public SourceType Type { get; set; }

        /// <summary>
        /// Whether the resource Free | Requires registration | Paid only
        /// </summary>
        public Availability Availability { get; set; }

        /// <summary>
        /// Whether inforation is Not applicable (default) | Up-to-date | Somewhat up-to-date | Outdated
        /// </summary>
        public Relevance Relevance { get; set; }

        /// <summary>
        /// List of tags related to theme
        /// </summary>
        public ICollection<string> Tags { get; set; }
    }
}