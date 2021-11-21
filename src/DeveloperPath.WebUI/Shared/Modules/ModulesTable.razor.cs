using System;
using System.Collections.Generic;
using DeveloperPath.Shared.ClientModels;
using DeveloperPath.WebUI.Resources;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

namespace DeveloperPath.WebUI.Shared.Modules
{
    public partial class ModulesTable
    {
        private string _searchString;

        [Parameter] public List<Module> Data { get; set; }
        [Parameter] public bool IsAdminMode { get; set; }
        [Parameter] public EventCallback<Module> OnEditClick { get; set; }
        [Inject] public IStringLocalizer<LanguageResources> Localizer { get; set; }

        private bool Filter(Module module)
        {
            return true;
        }

        private void OnModuleEditClick(Module editModule)
        {
            if (OnEditClick.HasDelegate)
                OnEditClick.InvokeAsync(editModule);
        }
    }
}
