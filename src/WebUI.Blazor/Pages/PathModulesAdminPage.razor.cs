using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        [Parameter] public string Key { get; set; }
        [Inject] public IStringLocalizer<LanguageResources> localizer { get; set; }

        [Inject] public SnackbarHelper SnakbarHelper { get; set; }
        [Inject] public ModuleService ModuleService { get; set; }
        [Inject] public ISnackbar Snackbar { get; set; }

        private List<BreadcrumbItem> _breadCrumbs;

        private bool Filter(Module module)
        {
            return true;
        }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                _breadCrumbs = new List<BreadcrumbItem>
                {
                    new($"{localizer["paths"]}", href: "/administration/paths"),
                    new($"{localizer["PathModules"].Value.ToLower()}", href: $"/administration/paths/{Key}/modules")

                };
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

        private Task ShowModalAddModule()
        {
            throw new NotImplementedException();
        }
    }
}
