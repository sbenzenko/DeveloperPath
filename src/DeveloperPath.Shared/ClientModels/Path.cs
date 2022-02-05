using System;
using System.Collections.Generic;

namespace DeveloperPath.Shared.ClientModels
{
    /// <summary>
    /// Developer path information
    /// </summary>
    public class Path
    {
        /// <summary>
        /// Path ID
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Path title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// URI key
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// Path short summary
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Make path visible for users
        /// </summary>
        public bool IsVisible { get; set; }

        /// <summary>
        /// List of tags related to path
        /// </summary>
        public ICollection<string> Tags { get; set; }
    }
}
