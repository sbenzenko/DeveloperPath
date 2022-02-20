using System.Collections.Generic;
using System.Threading.Tasks;
using DeveloperPath.Shared.ClientModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;

namespace DeveloperPath.WebUI.Shared.Modules
{
    public partial class AddEditModuleModal
  {
        [Parameter] public Module Module { get; set; }
        [Parameter] public bool IsNew { get; set; }
        [CascadingParameter] MudDialogInstance MudDialog { get; set; }
        
        private string _newTag;
        private MudForm _form;

        private void RemoveChip(MudChip chip) => Module.Tags.Remove(chip.Text);

        private void AddTag()
        {
            var newTagWithoutSpaces = _newTag.Trim();
            if (Module.Tags.Contains(newTagWithoutSpaces)
                || string.IsNullOrWhiteSpace(newTagWithoutSpaces)) return;
            Module.Tags.Add(newTagWithoutSpaces);
            _newTag = string.Empty;
        }

        private void TagFieldKeyPressed(KeyboardEventArgs obj)
        {
            if (obj.Key == "Enter")
                AddTag();
        }

        protected override Task OnInitializedAsync()
        {
            if (IsNew)
                Module = new Module() { Tags = new List<string>() };
            return base.OnInitializedAsync();
        }

        private void Cancel() => MudDialog.Cancel();

        private void Save()
        {
            _form.Validate();
            if (_form.IsValid)
            {
                MudDialog.Close(DialogResult.Ok(Module));
            }
        }
    }
}
