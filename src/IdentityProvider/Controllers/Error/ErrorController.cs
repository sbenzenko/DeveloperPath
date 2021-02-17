using IdentityProvider.Models.Error;
using Microsoft.AspNetCore.Mvc;

namespace IdentityProvider.Controllers.Error
{
    public class ErrorController : Controller
    {
        [HttpGet]
        public IActionResult Index(ErrorModel model)
        {
            return View(model);
        }
    }
}
