using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using DeveloperPath.Shared;
using DeveloperPath.Shared.ClientModels;
using DeveloperPath.WebUI.Services.Common;
using DeveloperPath.WebUI.UIHelper;
using DeveloperPath.WebUI.UIHelpers;

using Microsoft.AspNetCore.Components;

namespace DeveloperPath.WebUI.Pages;

public partial class PathsPage
{
  private const int PAGE_SIZE = 10;

  [Inject] public PathService PathService { get; set; }
  [Inject] public SnackbarHelper SnackbarHelper { get; set; }

  private int _lastPageRequest;
  private State _state;
  public List<Path> Paths { get; set; }
  public PaginationMetadata PaginationMetadata { get; private set; }
  protected override async Task OnInitializedAsync()
  {
    await LoadDataAsync(1);
  }

  private async Task LoadDataAsync(int pageNum)
  {
    try
    {
      _lastPageRequest = pageNum;

      _state = State.Loading;
      var result = await PathService.GetListAsync(false, pageNum, PAGE_SIZE);
      Paths = result.Data;
      PaginationMetadata = result.Metadata;
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
    await LoadDataAsync(_lastPageRequest);
  }
  private async Task ChangePageAsync(int pageNum)
  {
    await LoadDataAsync(pageNum);
  }
}
