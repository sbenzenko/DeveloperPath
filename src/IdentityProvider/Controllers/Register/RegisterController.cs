using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using DeveloperPath.Infrastructure.EmailSender;
using EmailSender.Interfaces;
using IdentityModel;
using IdentityProvider.Models;
using IdentityProvider.Models.Error;
using IdentityProvider.Models.Register;
using IdentityServer4.Models;
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
        private readonly IEmailNotifier _emailNotifier;
        private readonly IEmailNotifierConfig _emailNotifierConfig;

        public RegisterController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailNotifier emailNotifier, IEmailNotifierConfig emailNotifierConfig)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailNotifier = emailNotifier;
            _emailNotifierConfig = emailNotifierConfig;
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
            var message = new StringBuilder();
            message.Append("<body>");
            message.AppendLine("<h3><strong>Подтвердите свою почту на сайте</strong></h3>");
            message.AppendLine("<h3>Вы зарегистрировались на сайте газовой компании </h3>");
            message.AppendLine(
                $"Пожалуйста, подтвердите свою почту <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>кликнув здесь!</a>.");
            message.Append("</body>");
            var email = new Email()
            {
                HtmlContent = message.ToString(),
                EmailSender = new DeveloperPath.Infrastructure.EmailSender.EmailSender()
                {
                    Email = _emailNotifierConfig.Email,
                    Name = _emailNotifierConfig.EmailUserName
                },
                Subject = "Подтверждение регистрации",
                Recipients = new[] {user.Email}
            };
            await _emailNotifier.SendEmailAsync(email);

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
