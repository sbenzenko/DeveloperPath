using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using MudBlazor;
using WebUI.Blazor.Resources;

namespace WebUI.Blazor.UIHelper
{
    public class SnackbarHelper
    {
        private readonly ISnackbar _snackbar;
        private readonly IStringLocalizer<ErrorResources> _errorLocalizer;

        public SnackbarHelper(ISnackbar snackbar, IStringLocalizer<ErrorResources> errorLocalizer)
        {
            _snackbar = snackbar;
            this._errorLocalizer = errorLocalizer;
        }


        public void PrintErrorDetails(IDictionary<string, string[]> errors)
        {
            foreach (var error in errors)
            {
                StringBuilder sb = new StringBuilder();
                //  sb.Append($"<b>{errorLocalizer["VALIDATION_ERROR"]}</b>");
                sb.Append("<br/>");
                sb.Append("<ul>");
                foreach (var details in error.Value)
                    sb.AppendLine($"<li>{details}</li>");
                sb.Append("</ul>");
                _snackbar.Add(sb.ToString(), Severity.Error);
            }
        }

        public void PrintNotFoundDetails(string key, string message)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"<b>{_errorLocalizer[key]}</b>");
            sb.Append("<br/>");
            sb.Append("<ul>");

            sb.AppendLine($"<li>{message}</li>");
            sb.Append("</ul>");
            _snackbar.Add(sb.ToString(), Severity.Error);
        }
    }
}
