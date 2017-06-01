using System;
using System.Linq.Expressions;
using Tasks.Data;

namespace Tasks.Web.Models
{
    public class TaskViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime StartDateTime { get; set; }
        public TimeSpan? Duration { get; set; }
        public string Author { get; set; }
        public string Location { get; set; }
        public static Expression<Func<Task, TaskViewModel>> ViewModel
        {
            get
            {
                return e => new TaskViewModel()
                {
                    Id = e.Id,
                    Title = e.Title,
                    StartDateTime = e.StartDateTime,
                    Duration = e.Duration,
                    Location = e.Location,
                    Author = e.Author.FullName
                };
            }
        }
    }
}