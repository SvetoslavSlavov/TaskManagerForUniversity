using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tasks.Web.Models
{
    public class UpcomingPassedTasksViewModel
    {
        public IEnumerable<TaskViewModel> UpcomingTasks { get; set; }
        public IEnumerable <TaskViewModel> PassedTasks { get; set; }
    }
}