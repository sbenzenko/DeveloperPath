﻿using System;
using System.Threading.Tasks;

using DeveloperPath.Shared.ClientModels;
using DeveloperPath.WebUI.Services.Common;
using DeveloperPath.WebUI.UIHelper;
using DeveloperPath.WebUI.UIHelpers;

using Microsoft.AspNetCore.Components;

namespace DeveloperPath.WebUI.Pages;

public partial class PathPage
{
  private State _state;

  [Inject] public PathService PathService { get; set; }
  [Inject] public SnackbarHelper SnackbarHelper { get; set; }
  [Parameter] public int Id { get; set; }
  public PathDetails Model { get; set; }

  protected override async Task OnInitializedAsync()
  {
    await LoadDataAsync();
  }

  private async Task LoadDataAsync()
  {
    try
    {
      _state = State.Loading;
      Model = await PathService.GetPathAsync(Id);
      _state = State.ContentReady;
    }
    catch (Exception ex)
    {
      SnackbarHelper.PrintError(ex.Message);
      _state = State.Error;
    }
  }

  private async Task Reload()
  {
    await LoadDataAsync();
  }
}
