using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation.Results;

namespace DeveloperPath.Application.Common.Exceptions
{
  /// <summary>
  /// Custom API exception for validation errors
  /// </summary>
  public class ValidationException : Exception
  {
    /// <summary>
    /// </summary>
    public ValidationException()
        : base("One or more validation failures have occurred.")
    {
      Errors = new Dictionary<string, string[]>();
    }

    /// <summary>
    /// </summary>
    /// <param name="failures"></param>
    public ValidationException(IEnumerable<ValidationFailure> failures)
        : this()
    {
      var failureGroups = failures
          .GroupBy(e => e.PropertyName, e => e.ErrorMessage);

      foreach (var failureGroup in failureGroups)
      {
        var propertyName = failureGroup.Key;
        var propertyFailures = failureGroup.ToArray();

        Errors.Add(propertyName, propertyFailures);
      }
    }

    /// <summary>
    /// List of errors
    /// </summary>
    public IDictionary<string, string[]> Errors { get; }
  }
}