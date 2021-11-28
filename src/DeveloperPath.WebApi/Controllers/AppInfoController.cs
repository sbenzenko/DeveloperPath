using DeveloperPath.BuildInfo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeveloperPath.WebApi.Controllers
{
    [Route("api/application-info")]
    public class AppInfoController: ApiController
    {
        [AllowAnonymous]
        [HttpGet]
        public ActionResult<BuildInfo.BuildInfo> GetBuildInfo()
        {
            return Ok(AppVersionInfo.GetBuildInfo());
        }
    }
}
