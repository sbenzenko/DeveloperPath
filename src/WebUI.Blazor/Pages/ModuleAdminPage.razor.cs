using System.Collections.Generic;
using DeveloperPath.Domain.Shared.ClientModels;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;
using WebUI.Blazor.Resources;

namespace WebUI.Blazor.Pages
{
    public partial class ModuleAdminPage
    {
        private string _searchString;
        private IEnumerable<Module> _modules;

        [Inject] public IDialogService DialogService { get; set; }
        [Inject] public IStringLocalizer<LanguageResources> localizer { get; set; }
        [Inject] public IStringLocalizer<ErrorResources> errorLocalizer { get; set; }
        [Inject] public ISnackbar Snackbar { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }
    }
}

