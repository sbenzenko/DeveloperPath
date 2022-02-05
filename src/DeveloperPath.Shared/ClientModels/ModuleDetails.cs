using System;
using System.Collections.Generic;
using DeveloperPath.Shared.Enums;

namespace DeveloperPath.Shared.ClientModels
{
    /// <summary>
    /// Detailed information about the module
    /// </summary>
    public class ModuleDetails
    {
        /// <summary>
        /// Module ID
        /// </summary>
        public Guid Id { get; init; }

        /// <summary>
        /// Module title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Module short summary
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Paths module attached to
        /// </summary>
        public ICollection<PathTitle> Paths { get; init; }

        /// <summary>
        /// Necessity level (Other (default) | Possibilities | Interesting | Good to know | Must know)
        /// </summary>
        public Necessity Necessity { get; set; }

        /// <summary>
        /// Sections that module consists of (may be empty)
        /// </summary>
        public ICollection<Section> Sections { get; init; }

        /// <summary>
        /// Themes that module consists of
        /// </summary>
        public ICollection<Theme> Themes { get; init; }

        /// <summary>
        /// Modules required to know before studying this module
        /// </summary>
        public ICollection<ModuleTitle> Prerequisites { get; init; }

        /// <summary>
        /// List of tags related to module
        /// </summary>
        public ICollection<string> Tags { get; set; }
    }
}
