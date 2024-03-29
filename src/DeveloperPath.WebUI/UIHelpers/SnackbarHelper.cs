﻿using DeveloperPath.WebUI.Resources;
using Microsoft.Extensions.Localization;
using MudBlazor;
using System.Collections.Generic;
using System.Text;

namespace DeveloperPath.WebUI.UIHelpers
{
  public class SnackbarHelper
  {
    private readonly ISnackbar _snackbar;
    private readonly IStringLocalizer<ErrorResources> _errorLocalizer;

    public SnackbarHelper(ISnackbar snackbar, IStringLocalizer<ErrorResources> errorLocalizer)
    {
      _snackbar = snackbar;
      _errorLocalizer = errorLocalizer;
    }

    public void PrintSuccess(string message)
    {
      _snackbar.Add(message, Severity.Success);
    }
    public void PrintError(string message)
    {
      _snackbar.Add(message, Severity.Error);
    }
    public void PrintWarning(string message)
    {
      _snackbar.Add(message, Severity.Warning);
    }

    public void PrintErrorDetails(IDictionary<string, string[]> errors)
    {
      foreach (var error in errors)
      {
        StringBuilder sb = new StringBuilder();
        //  sb.Append($"<b>{errorLocalizer["VALIDATION_ERROR"]}</b>");
        sb.Append("<br/>");
        sb.Append("<ul>");
        foreach (var details in error.Value)
          sb.AppendLine($"<li>{details}</li>");
        sb.Append("</ul>");
        _snackbar.Add(sb.ToString(), Severity.Error);
      }
    }

    public void PrintNotFoundDetails(string key, string message)
    {
      StringBuilder sb = new StringBuilder();
      sb.Append($"<b>{_errorLocalizer[key]}</b>");
      sb.Append("<br/>");
      sb.Append("<ul>");

      sb.AppendLine($"<li>{message}</li>");
      sb.Append("</ul>");
      _snackbar.Add(sb.ToString(), Severity.Error);
    }
  }
}
