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
            TempData["confirm"] = true;

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> VerifyRegister(string email, string token)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) 
                return BadRequest();

            await _userManager.ConfirmEmailAsync(user, token);
            await _signInManager.SignInAsync(user, true);

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
            {
                ModelState.AddModelError("", "Incorrect");
                return View(loginViewModel);
            }

            var isExistUser = await _userManager.FindByNameAsync(loginViewModel.Username);
            if (isExistUser == null)
            {
                ModelState.AddModelError("", "Incorrect");
                return View(loginViewModel);
            }

            var result = await _signInManager.PasswordSignInAsync(isExistUser, loginViewModel.Password, false, false);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Incorrect");
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
            {
                ModelState.AddModelError("Username", "Username invalid");
                return View();
            }

            var isUser = await _userManager.FindByNameAsync(resetViewModel.Username);
            if (isUser == null)
            {
                ModelState.AddModelError("Username", "Username not found");
                return View();
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(isUser);

            string link = Url.Action(nameof(VerifyReset), "Account", new { email = isUser.Email, token }, Request.Scheme, Request.Host.ToString());

            if (SendEmail(isUser.Email, "reset password", link))
            {
                ViewBag.ConfirmationMessage = "Confirmation message was sent to your email. Please check your email to reset password.";
            }
            else
            {
                ModelState.AddModelError("", "Couldn't send email.");
            }

            return View();
        }

        public async Task<IActionResult> VerifyReset(string email, string token)
        {
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(email))
                return BadRequest();

            var isUser = await _userManager.FindByEmailAsync(email);
            if (isUser == null)
                return BadRequest();

            await _userManager.ConfirmEmailAsync(isUser, token);

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VerifyReset(string email, string token, ChangePassViewModel changePassViewModel)
        {
            //if (!ModelState.IsValid)
            //{
            //    ModelState.AddModelError("", "Incorrect Password");
            //    return View(registerViewModel);
            //}

            var isExistUser = await _userManager.FindByEmailAsync(email);
            if (isExistUser == null)
            {
                ModelState.AddModelError("", "Email Not Found");
                return View(changePassViewModel);
            }

            var result = await _userManager.ResetPasswordAsync(isExistUser, token, changePassViewModel.Password);

            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description + " old token " + token + " new pass " + changePassViewModel.Password);
                }
                return View(changePassViewModel);
            }

            await _signInManager.SignInAsync(isExistUser, true);

            return RedirectToAction("Index", "Home");
        }

        public bool SendEmail(string receiver, string subject, string message)
        {
            try
            {
                var senderEmail = new MailAddress("heydarovnamiq@gmail.com", "Namik Heydarov");
                var receiverEmail = new MailAddress(receiver, "Receiver");
                var password = "Nhl99nhl";
                var sub = subject;
                var body = message;
                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(senderEmail.Address, password)
                };

                using (var mess = new MailMessage(senderEmail, receiverEmail)
                {
                    Subject = sub,
                    Body = body
                })
                {
                    smtp.Send(mess);
                }
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}
