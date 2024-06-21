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
        public async Task<ActionResult> Create(UserVM user)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    user.UserTypesList = await GetUserTypes();
                    return View(user);
                }

                User usr = new User();
                usr.Name = user.Name;
                

                if (PasswordUtility.PasswordCheck(user.Password))
                {
                    usr.Salt = PasswordUtility.GenerateSalt();
                    usr.Password = PasswordUtility.HashToSHA256(user.Password, usr.Salt);
                    usr.UserType = user.UserType;
                }
                else 
                {
                    usr.Errors.Add(new ValidationError("The password should be stronger.", Type.ErrorType.Business));
                    string errorMessage = "";
                    foreach (var error in usr.Errors)
                    {
                        errorMessage += error.Description + " ";
                    }
                    ViewBag.ErrorMessage = errorMessage;
                    user.UserTypesList = await GetUserTypes();
                    return View(user);
                }

                var result = await service.CreateUser(usr);

                if (result.Errors.Count != 0)
                {
                    string errorMessage = "";
                    foreach (var error in result.Errors)
                    {
                        errorMessage += error.Description + " ";
                    }
                    ViewBag.ErrorMessage = errorMessage;
                    user.UserTypesList = await GetUserTypes();
                    return View(user);
                }

                TempData["Success"] = "User Created successfully.";
                return RedirectToAction("Index", "User");
            }
            catch(Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                user.UserTypesList = await GetUserTypes();
                return View(user);
            }
        }

        // GET: UserController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            //check login
            var userName = HttpContext.Session.GetString("UserName");

            if (userName == null)
            {
                return RedirectToAction("Login", "Login");
            }

            try
            {
                User user = await service.GetUserById(id);

                UserVM userVM = new UserVM();

                userVM.Id = user.Id;
                userVM.Name = user.Name;
                userVM.UserTypesList = await GetUserTypes();

                return View(userVM);
            }
            catch (Exception ex)
            {
                return ShowError(ex);
            }
        }

        // POST: UserController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(UserVM user)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    user.UserTypesList = await GetUserTypes();
                    return View(user);
                }

                User usr = new User();

                usr.Id = user.Id;
                usr.Name = user.Name;
                usr.UserType = user.UserType;

            
                var result = await service.EditUser(usr);

                if (result.Errors.Count != 0)
                {
                    string errorMessage = "";
                    foreach (var error in result.Errors)
                    {
                        errorMessage += error.Description + " ";
                    }
                    ViewBag.ErrorMessage = errorMessage;
                    user.UserTypesList = await GetUserTypes();
                    return View(user);
                }

                TempData["Success"] = "User Edited successfully.";
                return RedirectToAction("Index", "User");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message.ToString();
                return View(user);
            }
        }

        //----------------------------------

        // GET: UserController/ChangePassword/5
        public async Task<ActionResult> ChangePassword(int id)
        {
            //check login
            var userName = HttpContext.Session.GetString("UserName");

            if (userName == null)
            {
                return RedirectToAction("Login", "Login");
            }

            try
            {
                User user = await service.GetUserById(id);

                UserVM userVM = new UserVM();

                userVM.Id = user.Id;
                userVM.Name = user.Name;

                return View(userVM);
            }
            catch (Exception ex)
            {
                return ShowError(ex);
            }
        }

        // POST: UserController/ChangePassword/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(UserVM user)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(user);
                }

                User usr = new User();

                usr.Id = user.Id;
                var password = user.Password;

                if (PasswordUtility.PasswordCheck(user.Password))
                {
                    usr.Salt = PasswordUtility.GenerateSalt();
                    usr.Password = PasswordUtility.HashToSHA256(user.Password, usr.Salt);
                    usr.UserType = user.UserType;
                }
                else
                {
                    throw new Exception("The password should be stronger.");
                }

                var result = await service.EditUserPassword(usr);

                if (result.Errors.Count != 0)
                {
                    string errorMessage = "";
                    foreach (var error in result.Errors)
                    {
                        errorMessage += error.Description + " ";
                    }
                    ViewBag.ErrorMessage = errorMessage;
                    return View(user);
                }

                TempData["Success"] = "User Edited successfully.";
                return RedirectToAction("Index", "User");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message.ToString();
                return View(user);
            }
        }




        //----------------------------------

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
            var types = await list.GetUserType();

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
