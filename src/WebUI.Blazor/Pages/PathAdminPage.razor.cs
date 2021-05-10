using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using DeveloperPath.Domain.Shared.ClientModels;
using Microsoft.Extensions.Localization;
using MudBlazor;
using Shared.ProblemDetails;
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

        private Path _editablePath;

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
                    foreach (var error in (e.ProblemDetails as UnprocessableEntityProblemDetails).Errors)
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
            }
            catch (Exception e)
            {
                Snackbar.Add(e.Message, Severity.Error);
            }
            StateHasChanged();
        }

        private async Task ChangeVisible(Path pathItem)
        {
            
        }
    }
}
