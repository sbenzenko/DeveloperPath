using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using DeveloperPath.Domain.Shared.ClientModels;
using Microsoft.Extensions.Localization;
using MudBlazor;
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
            }
            catch (Exception e)
            {
                Snackbar.Add(e.Message, Severity.Error);
            }

            StateHasChanged();
        }
    }
}
