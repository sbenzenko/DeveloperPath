using System.Collections.Generic;
using System.Threading.Tasks;
using DeveloperPath.Shared;
using DeveloperPath.Shared.ClientModels;
using DeveloperPath.WebUI.Services;
using DeveloperPath.WebUI.UIHelper;
using Microsoft.AspNetCore.Components;

namespace DeveloperPath.WebUI.Pages
{
    public partial class PathPage
    {
        [Inject] public PathService PathService { get; set; }
        private State _state;
        public List<Path> Paths { get; set; }
        public PaginationMetadata PaginationMetadata { get; private set; }
        protected override async Task OnInitializedAsync()
        {
            _state = State.Loading;
            var result = await PathService.GetListAnonymousAsync(true, 1, 10);
            Paths = result.Data;
            PaginationMetadata = result.Metadata;
            _state = State.ContentReady;
        }

        private async Task ChangePageAsync(int pageNum)
        {
            _state = State.Loading;
            var result = await PathService.GetListAnonymousAsync(true, pageNum, PaginationMetadata.PageSize);
            
            Paths = result.Data;
            PaginationMetadata = result.Metadata;
            _state = State.ContentReady;

            StateHasChanged();
        }
    }
}
