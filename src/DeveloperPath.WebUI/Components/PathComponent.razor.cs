using DeveloperPath.Shared.ClientModels;

using Microsoft.AspNetCore.Components;

namespace DeveloperPath.WebUI.Components;

public partial class PathComponent
{
  [Parameter]
  public Path Model { get; set; } = new();
}