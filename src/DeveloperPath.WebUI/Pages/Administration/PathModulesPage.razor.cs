﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using DeveloperPath.Shared.ClientModels;
using DeveloperPath.Shared.ProblemDetails;
using DeveloperPath.WebUI.Resources;
using DeveloperPath.WebUI.Services;
using DeveloperPath.WebUI.Services.Administration;
using DeveloperPath.WebUI.UIHelpers;

using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

using MudBlazor;

namespace DeveloperPath.WebUI.Pages.Administration;

public partial class PathModulesPage
{
  [Parameter] public string Key { get; set; }
  [Inject] public IStringLocalizer<LanguageResources> Localizer { get; set; }
  [Inject] public SnackbarHelper SnackbarHelper { get; set; }
  [Inject] public ModuleService ModuleService { get; set; }
  [Inject] public ISnackbar Snackbar { get; set; }

  private List<BreadcrumbItem> _breadCrumbs;
  private readonly List<Module> _pathsModules;

  protected override void OnInitialized()
  {
    try
    {
      _breadCrumbs =
        [
          new($"{Localizer["Paths"].Value.ToUpper()}", href: "/administration/paths"),
          new($"{Localizer["PathModules"].Value.ToUpper()}", href: $"/administration/paths/{Key}/modules")
        ];
    }
    catch (ApiError e)
    {
      switch (e.ProblemDetails.Status)
      {
        case 422:
          SnackbarHelper.PrintErrorDetails((e.ProblemDetails as UnprocessableEntityProblemDetails).Errors);
          break;
        case 404:
          {
            var notFound = e.ProblemDetails as NotFoundProblemDetails;
            SnackbarHelper.PrintNotFoundDetails(notFound.ErrorKey, notFound.Error);
            break;
          }
      }
    }
    catch (Exception e)
    {
      Console.WriteLine(e.GetType().Name);
    }
  }

  private Task ShowModalAddModule()
  {
    throw new NotImplementedException();
  }
}