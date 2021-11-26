using System.Collections.Generic;
using DeveloperPath.Shared.Enums;

namespace DeveloperPath.Shared.ClientModels
{
    /// <summary>
    /// Represents module (skill) of the path, e.g. Programming language, Databases, CI/CD. etc.
    /// </summary>
    public class Module
    {
        /// <summary>
        /// Module ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Module title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Module unique URI key
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Module short summary
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Paths module attached to
        /// </summary>
        public ICollection<PathTitle> Paths { get; set; }

        /// <summary>
        /// Necessity level (Other (default) | Possibilities | Interesting | Good to know | Must know)
        /// </summary>
        public Necessity Necessity { get; set; }

        /// <summary>
        /// Modules required to know before studying this module
        /// </summary>
        public ICollection<ModuleTitle> Prerequisites { get; set; }

        /// <summary>
        /// List of tags related to the module
        /// </summary>
        public ICollection<string> Tags { get; set; }

        /// <summary>
        /// Service field for Blazor Table
        /// </summary>
        public bool ShowDetails;
    }
}
