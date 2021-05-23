using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Shared.ClientModels;
using WebUI.Blazor.Resources;
using WebUI.Blazor.Services;

namespace WebUI.Blazor.Pages
{
    public partial class PathDeletedPage
    {
        private List<DeletedPath> _paths;
        private string _searchString;

        [Inject] public IStringLocalizer<LanguageResources> localizer { get; set; }
        [Inject] public PathService PathService { get; set; }


        protected override async Task OnInitializedAsync()
        {
            _paths = await PathService.GetDeletedListAsync();
        }
    }
}
