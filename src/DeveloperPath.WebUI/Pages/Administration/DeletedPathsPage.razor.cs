//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//using DeveloperPath.Shared.ClientModels;
//using DeveloperPath.Shared.ProblemDetails;
//using DeveloperPath.WebUI.Resources;
//using DeveloperPath.WebUI.Services;

//using Microsoft.AspNetCore.Components;
//using Microsoft.Extensions.Localization;

//using MudBlazor;

//namespace DeveloperPath.WebUI.Pages.Administration;

//public partial class DeletedPathsPage
//{
//  private List<DeletedPath> _paths;
//  private readonly string _searchString;

//  [Inject] public IStringLocalizer<LanguageResources> Localizer { get; set; }
//  [Inject] public IStringLocalizer<ErrorResources> ErrorLocalizer { get; set; }
//  [Inject] public PathService PathService { get; set; }
//  [Inject] public ISnackbar Snackbar { get; set; }
//  private List<BreadcrumbItem> _breadCrumbs;

//  private bool Filter(DeletedPath element)
//  {
//    if (string.IsNullOrWhiteSpace(_searchString))
//      return true;
//    return element.Title.Contains(_searchString, StringComparison.OrdinalIgnoreCase) ||
//           element.Key.Contains(_searchString, StringComparison.OrdinalIgnoreCase);
//  }

//  protected override async Task OnInitializedAsync()
//  {
//    try
//    {
//      _breadCrumbs =
//        [
//            new($"{Localizer["Paths"].Value.ToUpper()}", href: "/administration/paths"),
//            new($"{Localizer["DeletedPaths"].Value.ToUpper()}", href: "/administration/paths/deleted")
//        ];

//      var listWithMetadata = await PathService.GetDeletedListAsync();
//      _paths = listWithMetadata.Data;
//    }
//    catch
//    {
//      Snackbar.Add("Something went wrong", Severity.Error);
//    }
//  }

//  private async Task RestoreDeletedPath(DeletedPath path)
//  {
//    try
//    {
//      var res = await PathService.RestoreDeletedPathAsync(path);
//      var restored = _paths.FirstOrDefault(x => x.Id == res.Id);

//      if (restored != null)
//        _paths.Remove(restored);

//    }
//    catch (ApiError e)
//    {
//      if (e.ProblemDetails.Status == 422)
//      {
//        PrintErrorDetails((e.ProblemDetails as UnprocessableEntityProblemDetails).Errors);
//      }
//    }
//    catch (Exception e)
//    {
//      Snackbar.Add(e.Message, Severity.Error);
//    }
//  }

//  void PrintErrorDetails(IDictionary<string, string[]> errors)
//  {
//    foreach (var error in errors)
//    {
//      StringBuilder sb = new();
//      sb.Append($"<b>{ErrorLocalizer["VALIDATION_ERROR"]}</b>");
//      sb.Append("<br/>");
//      sb.Append("<ul>");
//      foreach (var details in error.Value)
//        sb.AppendLine($"<li>{details}</li>");
//      sb.Append("</ul>");
//      Snackbar.Add(sb.ToString(), Severity.Error);
//    }
//  }
//}