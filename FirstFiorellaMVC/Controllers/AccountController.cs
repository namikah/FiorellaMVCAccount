using FirstFiorellaMVC.Models;
using FirstFiorellaMVC.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
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

            await _signInManager.SignInAsync(user, false);

            return RedirectToAction("Index", "Home");
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

            if (SendEmail(isUser.Email, "reset password", "https://localhost:44328/Account/ChangePassword/?token=" + token + "&username=" + isUser.UserName))
            {
                ViewBag.ConfirmationMessage = "Confirmation message was sent to your email. Please check your email to reset password.";
            }
            else
            {
                ModelState.AddModelError("", "Couldn't send email.");
            }

            return View();
        }

        public async Task<IActionResult> ChangePassword(string token, string username)
        {
            var isUser =await _userManager.FindByNameAsync(username);
            if(isUser == null)
            {
                return BadRequest();
            }

            var isToken = await _userManager.VerifyUserTokenAsync(isUser, TokenOptions.DefaultEmailProvider, "What is Want", token);
            if (!isToken)
            {
                return BadRequest();
            }
            
            return View();
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
