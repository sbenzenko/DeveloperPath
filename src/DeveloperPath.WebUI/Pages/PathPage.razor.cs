using System.Collections.Generic;
using System.Threading.Tasks;
using DeveloperPath.Shared;
using DeveloperPath.Shared.ClientModels;
using DeveloperPath.WebUI.Services;
using Microsoft.AspNetCore.Components;

namespace DeveloperPath.WebUI.Pages
{
    public partial class PathPage
    {
        [Inject] public PathService PathService { get; set; }

        public List<Path> Paths { get; set; }
        public PaginationMetadata PaginationMetadata { get; private set; }
        protected override async Task OnInitializedAsync()
        {
            var result = await PathService.GetListAnonymousAsync(false);
            Paths = result.Data;
            PaginationMetadata = result.Metadata;
        }
    }
}
