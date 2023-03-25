using Invoices.BL.Models;
using Invoices.DAL.Extend;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Invoices.Controllers
{
    public class AccountController : Controller
    {
        #region Fields
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        #endregion

        #region Ctor
        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }
        #endregion


        #region Register

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM obj)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    var user = new ApplicationUser()
                    {

                        UserName = obj.Email,
                        Email = obj.Email,
                        IsAgree = obj.IsAgree
                    };

                    var AddUser = await userManager.CreateAsync(user, obj.Password);

                    if (AddUser.Succeeded)
                    {
                        return RedirectToAction("Login");
                    }
                    else
                    {
                        foreach (var item in AddUser.Errors)
                        {
                            ModelState.AddModelError("", item.Description);
                        }
                    }
                }

                return View(obj);
            }
            catch (Exception)
            {
                return View(obj);
            }
        }
        #endregion

        #region LogIn

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM obj)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var User = await userManager.FindByEmailAsync(obj.Email);
                    var CheckPassword = await signInManager.PasswordSignInAsync(User, obj.Password, obj.RememberMe, false);

                    if (CheckPassword.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Invalid UserName or Password");
                    }
                }

                return View();
            }
            catch (Exception)
            {
                return View();
            }
        }
        #endregion

        #region LogOff (Sign Out)

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }


        #endregion

        #region Forget Password

        [HttpGet]
        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordVM obj)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var User = await userManager.FindByEmailAsync(obj.Email);

                    if (User != null)
                    {

                        // Generate Token
                        var token = await userManager.GeneratePasswordResetTokenAsync(User);

                        // Generate Reset Link
                        var passwordResetLink = Url.Action("ResetPassword", "Account", new { Email = obj.Email, Token = token }, Request.Scheme);

                        //MailSender.SendMail(new MailVM() { Mail = model.Email, Title = "Reset Password - Route Academy", Message = passwordResetLink });

                        return RedirectToAction("ConfirmForgetPassword");
                    }
                }

                return View(obj);
            }
            catch (Exception)
            {
                return View(obj);
            }
        }

        [HttpGet]
        public IActionResult ConfirmForgetPassword()
        {
            return View();
        }

        #endregion


        #region Reset Password


        [HttpGet]
        public IActionResult ResetPassword(string Email, string Token)
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordVM model)
        {

            try
            {

                if (ModelState.IsValid)
                {

                    var user = await userManager.FindByEmailAsync(model.Email);

                    if (user != null)
                    {
                        var result = await userManager.ResetPasswordAsync(user, model.Token, model.Password);

                        if (result.Succeeded)
                        {
                            return RedirectToAction("ConfirmResetPassword");
                        }

                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }

                        return View(model);
                    }

                    return RedirectToAction("ConfirmResetPassword");

                }

                return View(model);

            }
            catch (Exception)
            {
                return View(model);
            }

        }

        [HttpGet]
        public IActionResult ConfirmResetPassword()
        {
            return View();
        }
        #endregion
    }
}
