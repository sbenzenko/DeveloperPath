using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeveloperPath.Shared;
using DeveloperPath.Shared.ClientModels;
using DeveloperPath.Shared.ProblemDetails;
using DeveloperPath.WebUI.Resources;
using DeveloperPath.WebUI.Services;
using DeveloperPath.WebUI.Shared;
using DeveloperPath.WebUI.UIHelper;
using DeveloperPath.WebUI.UIHelpers;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;

namespace DeveloperPath.WebUI.Pages
{
    public partial class PathAdminPage : ComponentBase
    {
        const int PAGE_SIZE = 5;
        [Inject] public PathService PathService { get; set; }
        [Inject] public IDialogService DialogService { get; set; }
        [Inject] public IStringLocalizer<LanguageResources> localizer { get; set; }
        [Inject] public SnackbarHelper SnackbarHelper { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }

        public List<Path> Paths { get; set; }
        private State _state;
        public PaginationMetadata PaginationMetadata { get; private set; }

        private int _lastPageRequest;
      
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

        private async Task ShowModalNewPath()
        {
            var parameters = new DialogParameters { ["IsNew"] = true };
            var dialog = DialogService.Show<AddEditPathModal>(localizer["NewPath"], parameters);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                await AddNewPathAsync(result.Data as Path);
            }
        }

        private async Task ShowModalEditingPath(Path path)
        {
            var parameters = new DialogParameters { ["IsNew"] = false, ["Path"] = path };
            var dialog = DialogService.Show<AddEditPathModal>(localizer["NewPath"], parameters);
            var result = await dialog.Result;

            if (!result.Cancelled)
            {
                await EditPathAsync(result.Data as Path);
            }
        }

        private async Task EditPathAsync(Path path)
        {
            try
            {
                var result = await PathService.EditPathAsync(path);
                var item = Paths.FirstOrDefault(x => x.Id == result.Id);
                if (item != null)
                {
                    item = result;
                }
                SnackbarHelper.PrintSuccess(localizer["PathUpdated"]);
            }
            catch (ApiError e)
            {
                if (e.ProblemDetails.Status == 422)
                {
                    SnackbarHelper.PrintErrorDetails((e.ProblemDetails as UnprocessableEntityProblemDetails).Errors);
                }
            }
            catch (Exception e)
            {
                SnackbarHelper.PrintError(e.Message);
            }
            StateHasChanged();
        }

        private async Task AddNewPathAsync(Path resultData)
        {
            try
            {
                var result = await PathService.AddNewPathAsync(resultData);
                Paths.Add(result);
                SnackbarHelper.PrintSuccess(localizer["PathCreated"]);
            }
            catch (ApiError e)
            {
                if (e.ProblemDetails.Status == 422)
                {
                    SnackbarHelper.PrintErrorDetails((e.ProblemDetails as UnprocessableEntityProblemDetails).Errors);
                }
            }
            catch (Exception e)
            {
                SnackbarHelper.PrintError(e.Message);
            }
            StateHasChanged();
        }




        private async Task ChangePathVisibilityAsync(Path pathItem)
        {
            try
            {
                var result = await PathService.ChangeVisibility(pathItem);
                var changed = Paths.FirstOrDefault(x => x.Id == pathItem.Id);
                if (changed != null)
                    changed.IsVisible = result.IsVisible;

                StateHasChanged();
            }
            catch (Exception e)
            {
                SnackbarHelper.PrintError(e.Message);
            }
        }

        private async Task DeletePath(Path pathItem)
        {
            try
            {
                var result = await PathService.DeletePath(pathItem);
                if (result)
                {
                    Paths.Remove(pathItem);
                    SnackbarHelper.PrintWarning(localizer["PathDeleted"]);
                }
                StateHasChanged();
            }
            catch (Exception e)
            {
                SnackbarHelper.PrintError(e.Message);
            }
        }

        private void GoToDeleted()
        {
            NavigationManager.NavigateTo("/administration/paths/deleted");
        }

        private void GoToModules(Path pathItem)
        {
            NavigationManager.NavigateTo($"/administration/paths/{pathItem.Key}/modules");
        }
    }
}
