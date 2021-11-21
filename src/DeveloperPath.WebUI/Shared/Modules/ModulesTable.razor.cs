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

        [Parameter]

        public List<Module> Data { get; set; }

        [Inject] public IStringLocalizer<LanguageResources> Localizer { get; set; }

        private bool Filter(Module module)
        {
            return true;
        }
    }
}
