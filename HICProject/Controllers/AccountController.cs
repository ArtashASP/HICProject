using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using HICProject.Models;
using HICProject.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models.HIC;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HICProject.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public AccountController(UserManager<ApplicationUser> userManager,
                                 SignInManager<ApplicationUser> signInManager,
                                 RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [AllowAnonymous]
        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> IsEmailInUse(string email)
        {
            var user = await userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return Json(true);
            }
            else
            {
                return Json($"Email {email} is already in use!");
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel reg)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = reg.Email,
                    Email = reg.Email,
                    City = reg.City
                };
                var result = await userManager.CreateAsync(user, reg.Password);

                if (result.Succeeded)
                {
                    if (reg.Role == 0)
                    {
                        var role = await roleManager.FindByIdAsync("2a797b60-43b3-42cc-a074-01b96b306fb9");
                        IdentityResult Roleresult = null;
                        Roleresult = await userManager.AddToRoleAsync(user, role.Name);
                        await signInManager.SignInAsync(user, isPersistent: false);
                        return RedirectToAction("AddStaff", "home");
                    }
                    else
                    {
                        var role = await roleManager.FindByIdAsync("f2d31fa8-af20-4d2e-9e24-ad893cda571f");
                        IdentityResult Roleresult = null;
                        Roleresult = await userManager.AddToRoleAsync(user, role.Name);
                        await signInManager.SignInAsync(user, isPersistent: false);
                        return RedirectToAction("AddPatient", "home");
                    }
                }
                foreach (var err in result.Errors)
                {
                    ModelState.AddModelError("", err.Description);
                }
            }

            return View(reg);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("start", "home");
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Login(string returnUrl)
        {
            ViewLoginModel model = new ViewLoginModel()
            {
                ReturnUrl = returnUrl,
                ExternalLogins = (await signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
            };

            return View(model);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(ViewLoginModel reg, string returnUrl)
        {
            reg.ReturnUrl = returnUrl;
            reg.ExternalLogins = (await signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(reg.Email, reg.Password, reg.RememberMe, false);

                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(returnUrl) /*&& Url.IsLocalUrl(returnUrl)*/) //1-rd zev
                    {
                        return LocalRedirect(returnUrl); //2-rd zev
                    }
                    else
                    {
                        return RedirectToAction("start", "home");
                    }
                }
                ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
            }

            return View(reg);
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult ExternalLogin(string provider, string returnUrl)
        {
            var redirectUrl = Url.Action("ExternalLoginCallBack", "Account",
                                     new { ReturnUrl = returnUrl });
            var properties = signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return new ChallengeResult(provider, properties);
        }

        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallBack(string returnUrl = null, string remoteError = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            ViewLoginModel viewLoginModel = new ViewLoginModel
            {
                ReturnUrl = returnUrl,
                ExternalLogins = (await signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
            };

            if (remoteError != null)
            {
                ModelState.AddModelError(string.Empty, $"Error from external provider: {remoteError}");

                return View("Login", viewLoginModel);
            }

            var info = await signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                ModelState.AddModelError(string.Empty, "Error login external login information.");

                return View("Login", viewLoginModel);
            }

            var signInResult = await signInManager.ExternalLoginSignInAsync(info.LoginProvider,
                                    info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            if (signInResult.Succeeded)
            {
                return LocalRedirect("~/Home/start");
            }
            else
            {
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);

                if (email != null)
                {
                    var user = await userManager.FindByEmailAsync(email);

                    await userManager.AddLoginAsync(user, info);
                    await signInManager.SignInAsync(user, isPersistent: false);

                    return LocalRedirect(returnUrl);
                }
            }
            ViewBag.ErrorTitle = $"Email calin not received from: {info.LoginProvider}";

            return View("Error");
        }
    }
}