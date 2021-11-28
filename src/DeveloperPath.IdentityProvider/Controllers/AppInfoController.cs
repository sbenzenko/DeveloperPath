using DeveloperPath.BuildInfo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeveloperPath.WebApi.Controllers
{
    [Route("/application-info")]
    public class AppInfoController: ControllerBase
    {
        [AllowAnonymous]
        [HttpGet]
        public ActionResult<BuildInfo.BuildInfo> GetBuildInfo()
        {
            return Ok(AppVersionInfo.GetBuildInfo());
        }
    }
}
