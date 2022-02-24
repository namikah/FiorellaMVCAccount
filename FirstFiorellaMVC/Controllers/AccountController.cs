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
                ModelState.AddModelError("", "Error");
                return View(loginViewModel);
            }

            var isExistUser = await _userManager.FindByNameAsync(loginViewModel.Username);
            if (isExistUser == null)
            {
                ModelState.AddModelError("Username", "Not Found");
                return View(loginViewModel);
            }

            var user = new User()
            {
                UserName = loginViewModel.Username,
            };

            //var result = await _signInManager.PasswordSignInAsync(user, loginViewModel.Password, false, false);
            //if (!result.Succeeded)
            //{
            //    ModelState.AddModelError("Password", "Incorrect Password");
            //    return View(loginViewModel);
            //}

            //var result = await _signInManager.CheckPasswordSignInAsync(user, loginViewModel.Password, false);
            //if (!result.Succeeded)
            //{
            //    ModelState.AddModelError("Password", "Incorrect Password");
            //    return View(loginViewModel);
            //}

            //var result = await _userManager.CheckPasswordAsync(user, loginViewModel.Password);

            //if (!result)
            //{
            //    ModelState.AddModelError("Password", "Incorrect Password");
            //    return View(loginViewModel);
            //}


            await _signInManager.SignInAsync(user, false);

            return RedirectToAction("Index", "Home");
        }

        public ActionResult SendEmail()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SendEmail(string receiver, string subject, string message)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var senderEmail = new MailAddress("heydarovnamiq@gmail.com", "Namik Heydarov");
                    var receiverEmail = new MailAddress("namikah@code.edu.az", "Receiver");
                    var password = "Nhl99nhl";
                    var sub = "Test Message";
                    var body = "Bu bir test message-dir";
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
                    return View();
                }
            }
            catch (Exception)
            {
                ViewBag.Error = "Some Error";
            }
            return View();
        }
    }
}
