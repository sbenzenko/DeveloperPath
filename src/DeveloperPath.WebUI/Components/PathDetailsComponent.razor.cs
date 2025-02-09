using DeveloperPath.Shared.ClientModels;

using Microsoft.AspNetCore.Components;

namespace DeveloperPath.WebUI.Components;

public partial class PathDetailsComponent
{
  [Parameter]
  public PathDetails Model { get; set; } = new();
}