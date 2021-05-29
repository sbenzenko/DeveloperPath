using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DeveloperPath.Domain.Shared.ClientModels;
using DeveloperPath.Domain.Shared.ProblemDetails;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;
using Shared.ClientModels;
using Shared.ProblemDetails;
using WebUI.Blazor.Resources;
using WebUI.Blazor.Services;
using WebUI.Blazor.UIHelper;

namespace WebUI.Blazor.Pages
{
    public partial class PathModulesAdminPage
    {
        private string _searchString;
        private IEnumerable<Module> _pathsModules;
        private bool _shouldShowLoading = true;
        [Parameter] public string Key { get; set; }
        [Inject] public IStringLocalizer<LanguageResources> localizer { get; set; }

        [Inject] public SnackbarHelper SnakbarHelper { get; set; }
        [Inject] public ModuleService ModuleService { get; set; }
        [Inject] public ISnackbar Snackbar { get; set; }
        private bool Filter(Module module)
        {
            //throw new System.NotImplementedException();
            return true;
        }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                _pathsModules = await ModuleService.GetListAsync(Key);
            }
            catch (ApiError e)
            {
                switch (e.ProblemDetails.Status)
                {
                    case 422:
                        SnakbarHelper.PrintErrorDetails((e.ProblemDetails as UnprocessableEntityProblemDetails).Errors);
                        break;
                    case 404:
                    {
                        var notFound = e.ProblemDetails as NotFoundProblemDetails;
                        SnakbarHelper.PrintNotFoundDetails(notFound.ErrorKey, notFound.Error);
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.GetType().Name);
            }
        }
    }
}
