using System;
using DeveloperPath.Domain.Shared.ClientModels;

namespace Shared.ClientModels
{
    public class DeletedPath: Path
    {
        /// <summary>
        /// Path deletion date
        /// </summary>
        public DateTime? Deleted { get; set; }
    }
}
