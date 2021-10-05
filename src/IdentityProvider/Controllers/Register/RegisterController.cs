using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using IdentityModel;
using IdentityProvider.Models;
using IdentityProvider.Models.Error;
using IdentityProvider.Models.Register;
using IdentityProvider.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Serilog;
 

namespace IdentityProvider.Controllers.Register
{
    public class RegisterController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IServiceBusSenderService _serviceBusSenderService;

        public RegisterController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, 
            IServiceBusSenderService serviceBusSenderService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _serviceBusSenderService = serviceBusSenderService;
        }

        [HttpGet]
        public IActionResult Register(string returnUrl)
        {
            var vm = BuildRegisterViewModel(returnUrl);
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser() {UserName = model.Login, Email = model.Email, FullName = model.Name};
                Log.Information($"Register user with login {model.Login} started");
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    Log.Information($"Register user with login {model.Login} succeeded");
                    Log.Information($"Creating claims for user with login {model.Login} started");
                    result = await _userManager.AddClaimsAsync(user, new Claim[]
                    {
                        new Claim(JwtClaimTypes.Name, user.FullName),
                        new Claim(JwtClaimTypes.Email, user.Email)
                    });
                }

                if (result.Succeeded)
                {
                    Log.Information("User created a new account with password and claims.");
                    if (_userManager.Options.SignIn.RequireConfirmedEmail)
                    {
                        return RedirectToAction("RegisterConfirmation",
                            new RegisterConfirmationModel()
                            {
                                Login = model.Login,
                                ReturnUrl = model.ReturnUrl
                            });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(model.ReturnUrl);
                    }
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            else
            {
                Log.Information($"Registration is fault user with login {model.Login}");
                return View(model);
            }

            return View(model);
        }

        public async Task<IActionResult> RegisterConfirmation(RegisterConfirmationModel model)
        {
            var user = await _userManager.FindByNameAsync(model.Login);
            if (user == null)
            {
                return RedirectToAction("Index", "Error", new ErrorModel(model.ReturnUrl));
            }

            // Once you add a real email sender, you should remove this code that lets you confirm the account
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = Url.ActionLink(nameof(ConfirmEmail), "Register",
                new {userId = user.Id, code, returnUrl = model.ReturnUrl});

            await _serviceBusSenderService.SendMessageToEmailQueueAsync(callbackUrl, user.Email);
             
            return View(model);
        }

        public async Task<IActionResult> ConfirmEmail(string userId, string code, string returnUrl)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(code))
            {
                var error = new ErrorModel(returnUrl);
                error.Errors.Add("Не верные данные");
                return RedirectToAction("Index", "Error", error);
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"Не удалось найти такого пользователя в системе '{userId}'.");
            }
            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, code);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, true);
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Error", new ErrorModel(returnUrl));
            }
        }

        private RegisterModel BuildRegisterViewModel(string returnUrl)
        {
            return new RegisterModel()
            {
                ReturnUrl = returnUrl
            };
        }
    }
}
