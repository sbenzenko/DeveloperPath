using DeveloperPath.Shared.ClientModels;
using DeveloperPath.WebUI.Services;
using DeveloperPath.WebUI.UIHelper;
using DeveloperPath.WebUI.UIHelpers;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DeveloperPath.WebUI.Pages
{
    public partial class PathPage : ComponentBase
  {
        [Inject] public PathService PathService { get; set; }
        [Inject] public ModuleService ModuleService { get; set; }
        [Inject] public SnackbarHelper SnackbarHelper { get; set; }
        [Parameter] public string PathKey { get; set; }

        private State _state;
        public Path Path { get; set; }
        public List<Module> Modules { get; set; }
        protected override async Task OnInitializedAsync()
        {
            await LoadDataAsync(PathKey);
        }

        private async Task LoadDataAsync(string pathKey)
        {
            try
            {
                _state = State.Loading;
                Path = await PathService.GetAnonymousAsync(pathKey);
                if (Path is null)
                {
                    _state = State.NotFound;
                    return;
                }

                var result = await ModuleService.GetListAsync(Path.Id);
                Modules = result.Data;
                _state = State.ContentReady;
            }
            catch (Exception ex)
            {
                SnackbarHelper.PrintError(ex.Message);
                _state = State.Error;
            }
        }

        private async Task Reload()
        {
            await LoadDataAsync(PathKey);
        }

    }
}
