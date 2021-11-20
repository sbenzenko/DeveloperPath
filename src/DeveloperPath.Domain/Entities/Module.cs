using System;
using System.Collections.Generic;
using DeveloperPath.Domain.Common;
using DeveloperPath.Shared.Enums;

namespace DeveloperPath.Domain.Entities
{
    /// <summary>
    /// Represents module (skill) of the path, e.g. Programming language, Databases, CI/CD. etc.
    /// </summary>
    public record Module : AuditableEntity
    {
        /// <summary>
        /// Module URI key
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Module URI key
        /// </summary>
        public DateTime? Deleted { get; set; }

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
        public ICollection<Path> Paths { get; init; }

        /// <summary>
        /// Necessity level
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
        public ICollection<Module> Prerequisites { get; init; }

        /// <summary>
        /// List of tags related to module
        /// </summary>
        public ICollection<string> Tags { get; set; }
    }
}
