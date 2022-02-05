using System;

namespace DeveloperPath.Domain.Common
{
  /// <summary>
  /// Abstract class to add tracking to entities
  /// </summary>
  public abstract record AuditableEntity
  {
    /// <summary>
    /// ID
    /// </summary>
    public Guid Id { get; init; }

    // TODO: change to identity User
    /// <summary>
    /// Author of the entity
    /// </summary>
    public string CreatedBy { get; set; }

    /// <summary>
    /// Datetime entity created
    /// </summary>
    public DateTime Created { get; set; }

    /// <summary>
    /// Author who chnaged the entity
    /// TODO: change to identity User
    /// </summary>
    public string LastModifiedBy { get; set; }

    /// <summary>
    /// Datetime entity last changed
    /// </summary>
    public DateTime? LastModified { get; set; }
  }
}
