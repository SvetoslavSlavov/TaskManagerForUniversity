using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Tasks.Service;

namespace Tasks.Data
{
    public class ApplicationDbContext :
        IdentityDbContext<ApplicationUser>
    {
        public IDbSet<Service.Task> Tasks { get; set; }
        public IDbSet<Comment> Comments { get; set; }
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
        
    }
}