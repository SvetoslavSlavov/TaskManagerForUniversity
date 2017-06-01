
using Microsoft.AspNet.Identity;
using System;
using System.Linq;
using System.Web.Mvc;
using Tasks.Data;
using Tasks.Web.Extensions;
using Tasks.Web.Models;

namespace Tasks.Web.Controllers
{
    [Authorize]
    public class TasksController : BaseController
    {
        public ActionResult My()
        {
            string currentUserId = this.User.Identity.GetUserId();
            var tasks = this.db.Tasks
                .Where(e => e.AuthorId == currentUserId)
                .OrderBy(e => e.StartDateTime)
                .Select(TaskViewModel.ViewModel);

            var upcomingTasks = tasks.Where(e => e.StartDateTime > DateTime.Now);
            var passedTasks = tasks.Where(e => e.StartDateTime <= DateTime.Now);
            return View(new UpcomingPassedTasksViewModel()
            {
                UpcomingTasks = upcomingTasks,
                PassedTasks = passedTasks
            });
        }

        [HttpGet]
        public ActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TaskInputModel model)
        {
            if (model != null && this.ModelState.IsValid)
            {
                var e = new Task()
                {
                    AuthorId = this.User.Identity.GetUserId(),
                    Title = model.Title,
                    StartDateTime = model.StartDateTime,
                    Duration = model.Duration,
                    Description = model.Description,
                    Location = model.Location,
                    IsPublic = model.IsPublic
                };
                this.db.Tasks.Add(e);
                this.db.SaveChanges();
                this.AddNotification("Task created.", NotificationType.INFO);
                return this.RedirectToAction("My");
            }

            return this.View(model);
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var taskToEdit = this.LoadTask(id);
            if (taskToEdit == null)
            {
                this.AddNotification("Cannot edit task #" + id, NotificationType.ERROR);
                return this.RedirectToAction("My");
            }

            var model = TaskInputModel.CreateFromTask(taskToEdit);
            return this.View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, TaskInputModel model)
        {
            var taskToEdit = this.LoadTask(id);
            if (taskToEdit == null)
            {
                this.AddNotification("Cannot edit task #" + id, NotificationType.ERROR);
                return this.RedirectToAction("My");
            }

            if (model != null && this.ModelState.IsValid)
            {
                taskToEdit.Title = model.Title;
                taskToEdit.StartDateTime = model.StartDateTime;
                taskToEdit.Duration = model.Duration;
                taskToEdit.Description = model.Description;
                taskToEdit.Location = model.Location;
                taskToEdit.IsPublic = model.IsPublic;

                this.db.SaveChanges();
                this.AddNotification("Task edited.", NotificationType.INFO);
                return this.RedirectToAction("My");
            }

            return this.View(model);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var taskToDelete = this.LoadTask(id);
            if (taskToDelete == null)
            {
                this.AddNotification("Cannot delete task #" + id, NotificationType.ERROR);
                return this.RedirectToAction("My");
            }

            var model = TaskInputModel.CreateFromTask(taskToDelete);
            return this.View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, TaskInputModel model)
        {
            var taskToDelete = this.LoadTask(id);
            if (taskToDelete == null)
            {
                this.AddNotification("Cannot delete task #" + id, NotificationType.ERROR);
                return this.RedirectToAction("My");
            }

            this.db.Tasks.Remove(taskToDelete);
            this.db.SaveChanges();
            this.AddNotification("Task deleted.", NotificationType.INFO);
            return this.RedirectToAction("My");
        }

        private Task LoadTask(int id)
        {
            var currentUserId = this.User.Identity.GetUserId();
            var isAdmin = this.IsAdmin();
            var taskToEdit = this.db.Tasks
                .Where(e => e.Id == id)
                .FirstOrDefault(e => e.AuthorId == currentUserId || isAdmin);
            return taskToEdit;
        }
    }
}