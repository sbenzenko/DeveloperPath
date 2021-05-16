using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using WebUI.Admin.Models;

namespace WebUI.Admin.Controllers
{
  [Authorize]
  public class AdminController : Controller
  {
    private readonly ILogger<AdminController> _logger;

    public AdminController(ILogger<AdminController> logger)
    {
      _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
      await WriteOutIdentityInfo();

      return View();
    }

    public IActionResult Privacy()
    {
      return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
      return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    private async Task WriteOutIdentityInfo()
    {
      var identityToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.IdToken);
      Debug.WriteLine($"Identity token: {identityToken}");

      foreach (var claim in User.Claims)
        Debug.WriteLine($"Claim type: {claim.Type} - claim value: {claim.Value}");
    }
  }
}
