using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Threading.Tasks;
using Tasks.Data;

namespace Tasks.Web.Models
{
    public class TaskInputModel
    {
        [Required(ErrorMessage ="Task title is required.")]
        [StringLength(200,ErrorMessage ="The {0} most be between {2} and {1} character long",
            MinimumLength =1)]
        [Display(Name ="Title *")]
        public string Title { get; set; }
        
        [Display(Name ="Date and Time *")]
        public DateTime StartDateTime { get; set; }
        public TimeSpan? Duration { get; set; }
        public string Description { get; set; }
        [MaxLength(200)]
        public string Location { get; set; }
        [Display(Name ="Is Public")]
        public bool IsPublic { get; set; }

         public static TaskInputModel CreateFromTask(Data.Task e)
        {
            return new TaskInputModel()
            {
                Title = e.Title,
                StartDateTime = e.StartDateTime,
                Duration = e.Duration,
                Location = e.Location,
                Description = e.Description,
                IsPublic = e.IsPublic
            };
        }
    }
}