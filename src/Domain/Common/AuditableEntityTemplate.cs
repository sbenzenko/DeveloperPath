using System;

namespace DeveloperPath.Domain.Common
{
    //TODO: this class is from the project template. Remove it
    [Obsolete("This class is from the project template. Should not be used")]
    public abstract class AuditableEntityTemplate
    {
        public string CreatedBy { get; set; }

        public DateTime Created { get; set; }

        public string LastModifiedBy { get; set; }

        public DateTime? LastModified { get; set; }
    }
}
