using System;
using System.Linq;
using System.Web.Mvc;
using Tasks.Web.Models;
using Microsoft.AspNet.Identity;

namespace Tasks.Web.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            var tasks = this.db.Tasks
                .OrderBy(e => e.StartDateTime)
                .Where(e => e.IsPublic)
                .Select(TaskViewModel.ViewModel);

            var upcomingTasks = tasks.Where(e => e.StartDateTime > DateTime.Now);
            var passedTasks = tasks.Where(e => e.StartDateTime <= DateTime.Now);
            return View(new UpcomingPassedTasksViewModel()
            {
                UpcomingTasks=upcomingTasks,
                PassedTasks=passedTasks
            });
        }
        public ActionResult TaskDetailsById(int id)
        {
            var currentUserId = this.User.Identity.GetUserId();
            var isAdmin = this.IsAdmin();
            var taskDetails = this.db.Tasks
                .Where(e => e.Id == id)
                .Where(e => e.IsPublic || isAdmin || (e.AuthorId != null && e.AuthorId == currentUserId))
                .Select(TaskDetailsViewModel.ViewModel)
                .FirstOrDefault();

            var isOwner = (taskDetails != null && taskDetails.AuthorId != null &&
                taskDetails.AuthorId == currentUserId);
            this.ViewBag.CanEdit = isOwner || isAdmin;

            return this.PartialView("_TaskDetails", taskDetails);
        }
    }
}