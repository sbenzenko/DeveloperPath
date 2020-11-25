using System;

namespace DeveloperPath.Domain.Common
{
  /// <summary>
  /// Abstract class to add tracking to entities
  /// </summary>
  public abstract record AuditableEntity
  {
    /// <summary>
    /// Author of the entity
    /// TODO: change to identity User
    /// </summary>
    public string CreatedBy { get; init; }

    /// <summary>
    /// Datetime entity created
    /// </summary>
    public DateTime Created { get; init; }

    /// <summary>
    /// Author who chnaged the entity
    /// TODO: change to identity User
    /// </summary>
    public string LastModifiedBy { get; init; }

    /// <summary>
    /// Datetime entity last changed
    /// </summary>
    public DateTime? LastModified { get; init; }
  }
}
