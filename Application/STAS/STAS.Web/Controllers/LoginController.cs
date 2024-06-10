using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NuGet.Common;
using STAS.Model;
using STAS.Services;
using STAS.Web.Models;

namespace EpicSolutions.Web.Controllers
{
    public class LoginController : Controller
    {

        private readonly LoginService service = new();

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginVM vm)
        {

            if (string.IsNullOrEmpty(vm.UserName) && string.IsNullOrEmpty(vm.Password))
            {

                ViewBag.Error = "Employee Number and Password are required.";
                return View();
            }

            if (vm.UserName == null)
            {
                ViewBag.Error = "Invalid request.";
                return View();
            }

            var loginDTO = new LoginDTO()
            {
                UserName = vm.UserName,
                Password = vm.Password
            };

            var user = await service.Login(loginDTO);

            if (user != null)
            {
                // Create session
                HttpContext.Session.SetString("UserName", user.LoginUserName!);
                HttpContext.Session.SetString("Role", user.UserRole!);

                //Show message
                TempData["SuccessMessage"] = "Login successful.";

            }
            else
            {
                ViewBag.Error = "Invalid credentials.";

            }

            return View();
        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); // Clears the session
            return RedirectToAction("Index", "Home");
        }


    }
}
