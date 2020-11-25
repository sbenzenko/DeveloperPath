using System;

namespace DeveloperPath.Domain.Common
{
    //TODO: this class id from the template. Remove it
    public abstract class AuditableEntityTemplate
    {
        public string CreatedBy { get; set; }

        public DateTime Created { get; set; }

        public string LastModifiedBy { get; set; }

        public DateTime? LastModified { get; set; }
    }
}
