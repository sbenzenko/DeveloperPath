using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DeveloperPath.Shared.ClientModels;
using DeveloperPath.Shared.ProblemDetails;
using DeveloperPath.WebUI.Resources;
using DeveloperPath.WebUI.Services;
using DeveloperPath.WebUI.Shared;
using DeveloperPath.WebUI.UIHelper;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;

namespace DeveloperPath.WebUI.Pages
{
    public partial class ModulesAdminPage
    {
        [Inject] private ModuleService ModuleService { get; set; }
        public List<Module> Modules { get; set; }

        private State _state;

        [Inject] public IStringLocalizer<LanguageResources> localizer { get; set; }

        protected override async Task OnInitializedAsync()
        {
            _state = State.Loading;
            try
            {
                Modules = await ModuleService.GetListAsync();
                _state = State.ContentReady;
            }
            catch (Exception e)
            {
                _state = State.Error;
            }
        }

        private async Task ShowModalNewPath()
        {
            //var parameters = new DialogParameters { ["IsNew"] = true };
            //var dialog = DialogService.Show<AddEditPathModal>(localizer["NewPath"], parameters);
            //var result = await dialog.Result;
            //if (!result.Cancelled)
            //{
            //    await AddNewPathAsync(result.Data as Path);
            //}
        }

        private async Task ShowModalEditingPath(Path path)
        {
            //var parameters = new DialogParameters { ["IsNew"] = false, ["Path"] = path };
            //var dialog = DialogService.Show<AddEditPathModal>(localizer["NewPath"], parameters);
            //var result = await dialog.Result;

            //if (!result.Cancelled)
            //{
            //    await EditPathAsync(result.Data as Path);
            //}
        }

        private async Task EditPathAsync(Path path)
        {
            //try
            //{
            //    var result = await PathService.EditPathAsync(path);
            //    var item = Paths.FirstOrDefault(x => x.Id == result.Id);
            //    if (item != null)
            //    {
            //        item = result;
            //    }
            //    SnackbarHelper.PrintSuccess(localizer["PathUpdated"]);
            //}
            //catch (ApiError e)
            //{
            //    if (e.ProblemDetails.Status == 422)
            //    {
            //        SnackbarHelper.PrintErrorDetails((e.ProblemDetails as UnprocessableEntityProblemDetails).Errors);
            //    }
            //}
            //catch (Exception e)
            //{
            //    SnackbarHelper.PrintError(e.Message);
            //}
            //StateHasChanged();
        }

        private async Task AddNewPathAsync(Path resultData)
        {
            //try
            //{
            //    var result = await PathService.AddNewPathAsync(resultData);
            //    Paths.Add(result);
            //    SnackbarHelper.PrintSuccess(localizer["PathCreated"]);
            //}
            //catch (ApiError e)
            //{
            //    if (e.ProblemDetails.Status == 422)
            //    {
            //        SnackbarHelper.PrintErrorDetails((e.ProblemDetails as UnprocessableEntityProblemDetails).Errors);
            //    }
            //}
            //catch (Exception e)
            //{
            //    SnackbarHelper.PrintError(e.Message);
            //}
            //StateHasChanged();
        }
    }
}
