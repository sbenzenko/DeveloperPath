using System;

namespace DeveloperPath.Application.Common.Interfaces
{
  /// <summary>
  /// Date time interface
  /// </summary>
  public interface IDateTime
  {
    /// <summary>
    /// Current date time
    /// </summary>
    DateTime Now { get; }
  }
}
