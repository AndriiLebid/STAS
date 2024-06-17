using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using STAS.Model;
using STAS.Services;
using STAS.Web.Models;
using System.Diagnostics;

namespace STAS.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly UserService service = new();
        private readonly ListService list = new();

        // GET: UserController
        public async Task<ActionResult> Index()
        {
            List<User> users = await service.GetUserList();

            return View(users);
        }

        // GET: UserController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: UserController/Create
        public async Task<ActionResult> Create()
        {

            //check login
            var userName = HttpContext.Session.GetString("UserName");

            if (userName == null)
            {
                return RedirectToAction("Login", "Login");
            }

            try
            {

                UserVM user = new UserVM();
                user.UserTypesList = await GetUserTypes();

                return View(user);
            }
            catch (Exception ex)
            {
                return ShowError(ex);
            }

        }

        // POST: UserController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UserController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UserController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UserController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {

            //check login
            var userName = HttpContext.Session.GetString("UserName");

            if (userName == null)
            {
                return RedirectToAction("Login", "Login");
            }


            try
            {
                User? user = await service.GetUserById(id);

                if (user == null)
                {
                    new Exception("A deletion error occurred");
                }

                return View(user);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message.ToString();
                return RedirectToAction("Index", "User");
            }
 
        }

        // POST: UserController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(User user)
        {
            try
            {
                var result = await service.DeleteUserAsync(user.Id);
                if (result != 1)
                {
                    TempData["ErrorMessage"] = "A deletion error occurred";
                    return RedirectToAction("Index", "User");
                }

                TempData["Success"] = "The user has been deleted.";
                return RedirectToAction("Index", "User");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message.ToString();
                return RedirectToAction("Index", "User");
            }
        }


        #region Private Methods


        private ActionResult ShowError(Exception ex)
        {
            return View("Error", new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                Exception = ex
            });
        }


        private async Task<IEnumerable<SelectListItem>>? GetUserTypes()
        {
            var types = await new ListService().GetUserType();

            return types.Select(t =>
            new SelectListItem
            {
                Value = t.TypeId.ToString(),
                Text = t.TypeName
            }).ToList();
        }

        #endregion

    }
}
