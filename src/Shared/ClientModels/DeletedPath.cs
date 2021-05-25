using System;

namespace DeveloperPath.Domain.Shared.ClientModels
{
    public class DeletedPath : Path
    {
        /// <summary>
        /// Path deletion date
        /// </summary>
        public DateTime? Deleted { get; set; }
    }
}
