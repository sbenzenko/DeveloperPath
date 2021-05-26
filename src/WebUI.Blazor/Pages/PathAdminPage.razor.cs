using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using MudBlazor;

using DeveloperPath.Domain.Shared.ClientModels;
using DeveloperPath.Domain.Shared.ProblemDetails;
using WebUI.Blazor.Resources;
using WebUI.Blazor.Services;
using WebUI.Blazor.Shared;

namespace WebUI.Blazor.Pages
{

    public partial class PathAdminPage
    {
        [Inject] public PathService PathService { get; set; }
        [Inject] public IDialogService DialogService { get; set; }
        [Inject] public IStringLocalizer<LanguageResources> localizer { get; set; }
        [Inject] public IStringLocalizer<ErrorResources> errorLocalizer { get; set; }
        [Inject] public ISnackbar Snackbar { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }

 

        public List<Path> Paths { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                Paths = await PathService.GetListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
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
                Snackbar.Add(localizer["PathUpdated"], Severity.Success);
            }
            catch (ApiError e)
            {
                if (e.ProblemDetails.Status == 422)
                {
                    PrintErrorDetails((e.ProblemDetails as UnprocessableEntityProblemDetails).Errors);
                }
            }
            catch (Exception e)
            {
                Snackbar.Add(e.Message, Severity.Error);
            }
            StateHasChanged();
        }

        private async Task AddNewPathAsync(Path resultData)
        {
            try
            {
                var result = await PathService.AddNewPathAsync(resultData);
                Paths.Add(result);
                Snackbar.Add(localizer["PathCreated"], Severity.Success);
            }
            catch (ApiError e)
            {
                if (e.ProblemDetails.Status == 422)
                {
                    PrintErrorDetails((e.ProblemDetails as UnprocessableEntityProblemDetails).Errors);
                }
            }
            catch (Exception e)
            {
                Snackbar.Add(e.Message, Severity.Error);
            }
            StateHasChanged();
        }

        void PrintErrorDetails(IDictionary<string, string[]> errors)
        {
            foreach (var error in errors)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append($"<b>{errorLocalizer["VALIDATION_ERROR"]}</b>");
                sb.Append("<br/>");
                sb.Append("<ul>");
                foreach (var details in error.Value)
                    sb.AppendLine($"<li>{details}</li>");
                sb.Append("</ul>");
                Snackbar.Add(sb.ToString(), Severity.Error);
            }
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
                Snackbar.Add(e.Message, Severity.Error);
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
                    Snackbar.Add(localizer["PathDeleted"], Severity.Warning);
                }
                StateHasChanged();
            }
            catch (Exception e)
            {
                Snackbar.Add(e.Message, Severity.Error);
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
