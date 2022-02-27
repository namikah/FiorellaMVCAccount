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

            if (SendEmail(user, link))
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
                return BadRequest();

            await _userManager.ConfirmEmailAsync(user, token);
            await _signInManager.SignInAsync(user, user.EmailConfirmed);

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

            //token and user
            try
            {
                await TokenSaver(link);
                var url = Util<string>.MyReadFile(Constants.SeedDataPath, "VerifyToken.json");
                ViewBag.ConfirmationUrl = url;
            }
            catch
            {
                ModelState.AddModelError("", "Couldn't send email.");
            }

            return View();
        }

        public async Task<IActionResult> VerifyReset(string id, string token)
        {
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(id))
                return BadRequest();

            var isUser = await _userManager.FindByIdAsync(id);
            if (isUser == null)
                return BadRequest();

            await _userManager.ConfirmEmailAsync(isUser, token);

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
            {
                ModelState.AddModelError("", "Not Found");
                return View(passwordViewModel);
            }

            var result = await _userManager.ResetPasswordAsync(isExistUser, token, passwordViewModel.Password);

            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
                return View(passwordViewModel);
            }

            await _signInManager.SignInAsync(isExistUser, isExistUser.EmailConfirmed);

            return RedirectToAction("Index", "Home");
        }

        public bool SendEmail(User user, string link)
        {
            try
            {
                MailMessage msg = new MailMessage();
                msg.From = new MailAddress("codep320@gmail.com", "Fiorello");
                msg.To.Add(user.Email);
                string body = string.Empty;
                using (StreamReader reader = new StreamReader("wwwroot/template/verifyemail.html"))
                {
                    body = reader.ReadToEnd();
                }
                body = body.Replace("{{link}}", link);
                body = body.Replace("{{name}}", $"Welcome, {user.UserName.ToUpper()}");
                msg.Body = body;
                msg.Subject = "Verify";
                msg.IsBodyHtml = true;

                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.EnableSsl = true;
                smtp.Credentials = new NetworkCredential("codep320@gmail.com", "codeacademyp320");
                smtp.Send(msg);
            }
            catch
            {
                return false;
            }

            return true;
        }

        public async Task TokenSaver(string link)
        {
            await Util<string>.MyCreateFileAsync(link, Constants.SeedDataPath, "VerifyToken.json");
        }
    }
}
