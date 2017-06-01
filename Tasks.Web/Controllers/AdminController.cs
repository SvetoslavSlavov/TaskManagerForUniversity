using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using static Tasks.Data.Users;

namespace Tasks.Web.Controllers
{
    [OnlyFor(Rule = UserRoles.Admin)]
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult List()
        {
            UsersService users = ServiceFactory.GetUsersService();
            List<UserViewModel> model = new List<UserViewModel>();
            foreach (User user in users.GetAllUsers())
            {
                UserViewModel userVM = new UserViewModel
                {
                    Id = user.Id,
                    Username = user.Username,
                    FirstName = user.FirstName,
                    Role = user.Role
                };
                model.Add(userVM);
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(UserViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                UsersService users = ServiceFactory.GetUsersService();
                User user = new User
                {
                    Username = model.Username,
                    Password = model.Password,
                    FirstName = model.FirstName,
                    Role = model.Role
                };
                users.AddUser(user);
                this.TempData["State"] = "User is added!";
                return RedirectToAction("List", "Users");
            }
            else
            {
                return View(model);
            }
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            UsersService users = ServiceFactory.GetUsersService();
            User user = users.GetUserById(id);
            UserViewModel userVM = new UserViewModel
            {
                Id = user.Id,
                Username = user.Username,
                FirstName = user.FirstName,
                Role = user.Role
            };
            return View("~/Views/Users/Add.cshtml", userVM);
        }

        [HttpPost]
        public ActionResult Edit(int id, UserViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                UsersService users = ServiceFactory.GetUsersService();
                User user = new User
                {
                    Username = model.Username,
                    FirstName = model.FirstName,
                    Role = model.Role
                };
                users.AddUser(user);
                this.TempData["State"] = "User is updated!";
                return RedirectToAction("List", "Users");
            }
            else
            {
                return View("~/Views/Users/Add.cshtml", model);
            }
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            UsersService users = ServiceFactory.GetUsersService();
            User user = users.GetUserById(id);
            users.DeleteUser(user);
            TempData["DeleteMessage"] = "User is deleted!";
            return this.RedirectToAction("List");
        }
    }
}