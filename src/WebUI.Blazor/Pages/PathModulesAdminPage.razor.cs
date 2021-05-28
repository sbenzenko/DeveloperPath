using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DeveloperPath.Domain.Shared.ClientModels;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Shared.ClientModels;
using WebUI.Blazor.Resources;
using WebUI.Blazor.Services;

namespace WebUI.Blazor.Pages
{
    public partial class PathModulesAdminPage
    {
        private string _searchString;
        private IEnumerable<Module> _pathsModules;
        [Parameter] public string Key { get; set; }
        [Inject] public IStringLocalizer<LanguageResources> localizer { get; set; }
        [Inject] public ModuleService ModuleService { get; set; }
        private bool Filter(Module module)
        {
            //throw new System.NotImplementedException();
            return true;
        }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                _pathsModules = await ModuleService.GetListAsync(1);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
