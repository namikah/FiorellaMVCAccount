using Microsoft.AspNetCore.Mvc;
using System;

namespace FirstFiorellaMVC.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Index(int? statusCode)
        {
            return View(statusCode);
        }
    }
}
