using System;
using System.Collections.Generic;
using System.Linq;

namespace DeveloperPath.Application.Common.Models
{
  /// <summary>
  /// Identity service common result
  /// </summary>
  public class Result
  {
    internal Result(bool succeeded, IEnumerable<string> errors)
    {
      Succeeded = succeeded;
      Errors = errors.ToArray();
    }

    /// <summary>
    /// Success status
    /// </summary>
    public bool Succeeded { get; set; }

    /// <summary>
    /// List of errors
    /// </summary>
    public string[] Errors { get; set; }

    /// <summary>
    /// Result success
    /// </summary>
    /// <returns></returns>
    public static Result Success()
    {
      return new Result(true, Array.Empty<string>());
    }

    /// <summary>
    /// Result failure
    /// </summary>
    /// <param name="errors"></param>
    /// <returns></returns>
    public static Result Failure(IEnumerable<string> errors)
    {
      return new Result(false, errors);
    }
  }
}
