using Domain.Shared.Enums;

namespace Application.Shared.Dtos.Models
{
    /// <summary>
    /// Module section (logical group of themes, like a book chapter)
    /// </summary>
    public class SectionDto
    {
        /// <summary>
        /// Section ID
        /// </summary>
        public int Id { get; init; }

        /// <summary>
        /// Section Title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Necessity level (Other (default) | Possibilities | Interesting | Good to know | Must know)
        /// </summary>
        public NecessityLevel Necessity { get; set; }

        /// <summary>
        /// Position of section in module (0-based)
        /// </summary>
        public int Order { get; set; }
    }
}