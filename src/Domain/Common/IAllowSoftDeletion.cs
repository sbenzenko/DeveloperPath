using System;

namespace DeveloperPath.Domain.Common
{
    public interface IAllowSoftDeletion
    {
        /// <summary>
        /// Marks entity as deleted without physical deletion
        /// </summary>
        DateTime? Deleted { get; set; }
    }
}
