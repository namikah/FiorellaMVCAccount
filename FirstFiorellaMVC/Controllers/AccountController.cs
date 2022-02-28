using FirstFiorellaMVC.Data;
using FirstFiorellaMVC.Models;
using FirstFiorellaMVC.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace FirstFiorellaMVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;

        private readonly SignInManager<User> _signInManager;


        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager = null)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(registerViewModel);
            }

            var isExistUser = await _userManager.FindByNameAsync(registerViewModel.Username);
            if (isExistUser != null)
            {
                ModelState.AddModelError("Username", "Allready exist username");
                return View(registerViewModel);
            }

            var user = new User()
            {
                FullName = registerViewModel.Fullname,
                UserName = registerViewModel.Username,
                Email = registerViewModel.Email,
            };

            var result = await _userManager.CreateAsync(user, registerViewModel.Password);

            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
                return View(registerViewModel);
            }

            string token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            string link = Url.Action(nameof(VerifyRegister), "Account", new { email = user.Email, token }, Request.Scheme, Request.Host.ToString());

            if (MyEmailUtil.SendEmail(user, link))
            {
                TempData["confirm"] = true;
            }
            else
            {
                ModelState.AddModelError("", "email not send");
                return View(registerViewModel);
            }

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> VerifyRegister(string email, string token)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return RedirectToAction(nameof(Index), "Error", BadRequest());

            await _userManager.ConfirmEmailAsync(user, token);
            await _signInManager.SignInAsync(user, false);

            TempData["confirmed"] = true;

            return RedirectToAction(nameof(Index), "Home");
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid)
                return View(loginViewModel);

            var isExistUser = await _userManager.FindByNameAsync(loginViewModel.Username);
            if (isExistUser == null)
            {
                ModelState.AddModelError("", "Username or password incorrect");
                return View(loginViewModel);
            }

            var result = await _signInManager.PasswordSignInAsync(isExistUser, loginViewModel.Password, loginViewModel.RememberMe, false);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Username or password incorrect");
                return View(loginViewModel);
            }

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }

        public IActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetViewModel resetViewModel)
        {
            if (!ModelState.IsValid)
                return View();

            var isUser = await _userManager.FindByNameAsync(resetViewModel.Username);
            if (isUser == null)
            {
                ModelState.AddModelError("", "Username incorrect");
                return View();
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(isUser);

            string link = Url.Action(nameof(VerifyReset), "Account", new { id = isUser.Id, token }, Request.Scheme, Request.Host.ToString());

            //token link write and read from json file
            try
            {
                await MyFileUtil<string>.MyCreateFileAsync(link, Constants.SeedDataPath, "VerifyToken.json");
                var url = MyFileUtil<string>.MyReadFile(Constants.SeedDataPath, "VerifyToken.json");
                ViewBag.ConfirmationUrl = url;
            }
            catch
            {
                ModelState.AddModelError("", "id or token unreachable.");
            }

            return View();
        }

        public IActionResult VerifyReset(string id, string token)
        {
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(id))
                return RedirectToAction(nameof(Index), "Error", BadRequest());

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VerifyReset(string id, string token, PasswordViewModel passwordViewModel)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Incorrect");
                return View(passwordViewModel);
            }

            var isExistUser = await _userManager.FindByIdAsync(id);
            if (isExistUser == null)
                return RedirectToAction(nameof(Index), "Error", NotFound());

            var result = await _userManager.ResetPasswordAsync(isExistUser, token, passwordViewModel.Password);
            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
                return View(passwordViewModel);
            }

            return RedirectToAction(nameof(Login));
        }
    }
}
