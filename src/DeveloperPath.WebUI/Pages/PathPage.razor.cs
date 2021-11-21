using System.Collections.Generic;
using System.Threading.Tasks;
using DeveloperPath.Shared.ClientModels;
using DeveloperPath.WebUI.Services;
using Microsoft.AspNetCore.Components;

namespace DeveloperPath.WebUI.Pages
{
    public partial class PathPage
    {
        [Inject] public PathService PathService { get; set; }

        public List<Path> Paths { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Paths = await PathService.GetListAnonymousAsync();
        }
    }
}
