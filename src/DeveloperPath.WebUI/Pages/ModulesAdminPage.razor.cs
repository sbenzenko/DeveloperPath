﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeveloperPath.Shared.ClientModels;
using DeveloperPath.Shared.ProblemDetails;
using DeveloperPath.WebUI.Resources;
using DeveloperPath.WebUI.Services;
using DeveloperPath.WebUI.Shared.Modules;
using DeveloperPath.WebUI.UIHelper;
using DeveloperPath.WebUI.UIHelpers;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;

namespace DeveloperPath.WebUI.Pages
{
    public partial class ModulesAdminPage
    {
        [Inject] private ModuleService ModuleService { get; set; }
        [Inject] public IDialogService DialogService { get; set; }
        [Inject] public SnackbarHelper SnackbarHelper { get; set; }
        public List<Module> Modules { get; set; }

        private State _state;

        [Inject] public IStringLocalizer<LanguageResources> localizer { get; set; }

        protected override async Task OnInitializedAsync()
        {
            _state = State.Loading;
            try
            {
                Modules = await ModuleService.GetListAsync();
                _state = State.ContentReady;
            }
            catch (Exception e)
            {
                _state = State.Error;
            }
        }

        private async Task ShowModalNewModule()
        {
            var parameters = new DialogParameters { ["IsNew"] = true };
            var dialog = DialogService.Show<AddEditModuleModal>(localizer["NewModule"], parameters);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                await AddNewModuleAsync(result.Data as Module);
            }
        }

        private async Task ShowModalEditingPath(Module module)
        {
            var parameters = new DialogParameters { ["IsNew"] = false, ["Module"] = module };
            var dialog = DialogService.Show<AddEditModuleModal>(localizer["EditModule"], parameters);
            var result = await dialog.Result;

            if (!result.Cancelled)
            {
                await EditModuleAsync(result.Data as Module);
            }
        }



        private async Task EditModuleAsync(Module module)
        {
            try
            {
                var result = await ModuleService.EditModuleAsync(module);
                var item = Modules.FirstOrDefault(x => x.Id == result.Id);
                if (item != null)
                {
                    item = result;
                }
                SnackbarHelper.PrintSuccess(localizer["ModuleUpdated"]);
            }
            catch (ApiError e)
            {
                if (e.ProblemDetails.Status == 422)
                {
                    SnackbarHelper.PrintErrorDetails((e.ProblemDetails as UnprocessableEntityProblemDetails).Errors);
                }
            }
            catch (Exception e)
            {
                SnackbarHelper.PrintError(e.Message);
            }
            StateHasChanged();
        }

        private async Task AddNewModuleAsync(Module resultData)
        {
            //try
            //{
            //    var result = await PathService.AddNewPathAsync(resultData);
            //    Paths.Add(result);
            //    SnackbarHelper.PrintSuccess(localizer["PathCreated"]);
            //}
            //catch (ApiError e)
            //{
            //    if (e.ProblemDetails.Status == 422)
            //    {
            //        SnackbarHelper.PrintErrorDetails((e.ProblemDetails as UnprocessableEntityProblemDetails).Errors);
            //    }
            //}
            //catch (Exception e)
            //{
            //    SnackbarHelper.PrintError(e.Message);
            //}
            //StateHasChanged();
        }
    }
}
