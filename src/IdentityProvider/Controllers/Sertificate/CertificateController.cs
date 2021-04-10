using Microsoft.AspNetCore.Mvc;

namespace IdentityProvider.Controllers.Sertificate
{
    [Route("/.well-known/pki-validation/godaddy.html")]
    public class CertificateController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
