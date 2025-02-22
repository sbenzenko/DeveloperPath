﻿using System.Collections.Generic;

using DeveloperPath.Domain.Common;
using DeveloperPath.Shared.Enums;

namespace DeveloperPath.Domain.Entities;

/// <summary>
/// Particular theme of the module
/// </summary>
public record Theme : AuditableEntity
{
  /// <summary>
  /// Theme ID
  /// </summary>
  //public int Id { get; init; }

  /// <summary>
  /// Theme Title
  /// </summary>
  public string Title { get; set; }

  /// <summary>
  /// Theme short summary
  /// </summary>
  public string Description { get; set; }

  /// <summary>
  /// Id of module that theme is in
  /// </summary>
  public int ModuleId { get; set; }

  /// <summary>
  /// Module that theme is in
  /// </summary>
  public Module Module { get; init; }

  /// <summary>
  /// Section that theme is in (can be null)
  /// </summary>
  public Section Section { get; set; }

  /// <summary>
  /// Complexity level
  /// </summary>
  public Complexity Complexity { get; set; }

  /// <summary>
  /// Necessity level
  /// </summary>
  public Necessity Necessity { get; set; }

  /// <summary>
  /// Position of theme in module (0-based). 
  /// Multiple themes can have same order number (meaning they can be studied simultaneously)
  /// </summary>
  public int Order { get; set; }

  /// <summary>
  /// Sources fo this theme
  /// </summary>
  public IList<Source> Sources { get; init; }

  /// <summary>
  /// Themes required to know before studying this theme
  /// </summary>
  public ICollection<Theme> Prerequisites { get; init; }

  /// <summary>
  /// Related themes ("See also" section)
  /// </summary>
  public ICollection<Theme> Related { get; init; }

  /// <summary>
  /// List of tags related to theme
  /// </summary>
  public ICollection<string> Tags { get; set; }
}