using System.Collections.Generic;
using System.Threading.Tasks;
using DeveloperPath.Shared.ClientModels;
using DeveloperPath.WebUI.Services;
using Microsoft.AspNetCore.Components;

namespace DeveloperPath.WebUI.Pages
{
    public partial class ModulesAdminPage
    {
        [Inject] private ModuleService ModuleService { get; set; }
        public List<Module> Modules { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Modules = await ModuleService.GetListAsync();
        }
    }
}
