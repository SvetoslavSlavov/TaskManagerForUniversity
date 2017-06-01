using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Tasks.Data;

namespace Tasks.Web.Models
{
    public class TaskDetailsViewModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string AuthorId { get; set; }
        public IEnumerable<CommentViewModel> Comments { get; set; }
        public static Expression<Func<Task, TaskDetailsViewModel>> ViewModel
        {
            get
            {
                return e => new TaskDetailsViewModel()
                {
                    Id = e.Id,
                    Description = e.Descrioption,
                    Comments = e.Comments.AsQueryable().Select(CommentViewModel.ViewModel),
                    AuthorId = e.AuthorId
                };
            }
        }
    }
}