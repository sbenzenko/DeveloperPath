using System.Collections.Generic;
using System.Threading.Tasks;

using DeveloperPath.Shared.ClientModels;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

using MudBlazor;

namespace DeveloperPath.WebUI.Shared
{
  public partial class AddEditPathModal
  {
    [Parameter] public Path Path { get; set; }
    [Parameter] public bool IsNew { get; set; }
    [CascadingParameter] MudDialog MudDialog { get; set; }
    private string _newTag;
    private MudForm _form;

    void RemoveChip(MudChip<string> chip) => Path.Tags.Remove(chip.Text);

    private void AddTag()
    {
      var newTagWithoutSpaces = _newTag.Trim();
      if (Path.Tags.Contains(newTagWithoutSpaces)
          || string.IsNullOrWhiteSpace(newTagWithoutSpaces)) return;
      Path.Tags.Add(newTagWithoutSpaces);
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
        Path = new Path() { Tags = new List<string>() };
      return base.OnInitializedAsync();
    }

    private async Task Cancel() => await MudDialog.CloseAsync(DialogResult.Cancel());

    private async Task Save()
    {
      await _form.Validate();
      if (_form.IsValid)
        await MudDialog.CloseAsync(DialogResult.Ok(Path));
    }
  }
}
