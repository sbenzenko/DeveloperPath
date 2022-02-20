using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DeveloperPath.Shared.ClientModels;
using DeveloperPath.Shared.ProblemDetails;
using DeveloperPath.WebUI.Resources;
using DeveloperPath.WebUI.Services;
using DeveloperPath.WebUI.UIHelpers;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;

namespace DeveloperPath.WebUI.Pages
{
    public partial class PathModulesAdminPage : ComponentBase
  {
        [Parameter] public string Key { get; set; }
        [Inject] public IStringLocalizer<LanguageResources> localizer { get; set; }
        [Inject] public SnackbarHelper SnakbarHelper { get; set; }
        [Inject] public ModuleService ModuleService { get; set; }
        [Inject] public ISnackbar Snackbar { get; set; }

        private List<BreadcrumbItem> _breadCrumbs;
        private List<Module> _pathsModules;


        protected override async Task OnInitializedAsync()
        {
            try
            {
                _breadCrumbs = new List<BreadcrumbItem>
                {
                    new($"{localizer["Paths"].Value.ToUpper()}", href: "/administration/paths"),
                    new($"{localizer["PathModules"].Value.ToUpper()}", href: $"/administration/paths/{Key}/modules")

                };
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

        private Task ShowModalAddModule()
        {
            throw new NotImplementedException();
        }
    }
}
