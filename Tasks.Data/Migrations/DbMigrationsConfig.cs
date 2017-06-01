using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;

namespace Tasks.Data.Migrations
{
    public sealed class DbMigrationsConfig : 
        DbMigrationsConfiguration<Tasks.Data.ApplicationDbContext>
    {
        public DbMigrationsConfig()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }
        
        protected override void Seed(ApplicationDbContext context)
        {
            //Seed initial data only if the database is empty
            if (!context.Users.Any())
            {
                var adminEmail = "admin@admin.com";
                var adminUserName = adminEmail;
                var adminFullName = "System Administrator";
                var adminPassword = adminEmail;
                string adminRole = "Administrator";
                CreateAdminUser(context, adminEmail, adminUserName,
                    adminFullName, adminPassword, adminRole);
                CreateSeveralTasks(context);

            }
        }
        private void CreateAdminUser(ApplicationDbContext context, string adminEmail, string adminUserName, string adminFullName, string adminPassword, string adminRole)
        {
            // Create the "admin" user
            var adminUser = new ApplicationUser
            {
                UserName = adminUserName,
                FullName = adminFullName,
                Email = adminEmail
            };
            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);
            userManager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 1,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false,
            };
            var userCreateResult = userManager.Create(adminUser, adminPassword);
            if (!userCreateResult.Succeeded)
            {
                throw new Exception(string.Join("; ", userCreateResult.Errors));
            }

            // Create the "Administrator" role
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var roleCreateResult = roleManager.Create(new IdentityRole(adminRole));
            if (!roleCreateResult.Succeeded)
            {
                throw new Exception(string.Join("; ", roleCreateResult.Errors));
            }

            // Add the "admin" user to "Administrator" role
            var addAdminRoleResult = userManager.AddToRole(adminUser.Id, adminRole);
            if (!addAdminRoleResult.Succeeded)
            {
                throw new Exception(string.Join("; ", addAdminRoleResult.Errors));
            }
        }


        private void CreateSeveralTasks(ApplicationDbContext context)
        {
            context.Tasks.Add(new Task()
            {
                Title = "Party",
                StartDateTime = DateTime.Now.Date.AddDays(5).AddHours(21).AddMinutes(30)
            });
            context.Tasks.Add(new Task()
            {
                Title="Go to the bank",
                StartDateTime=DateTime.Now.Date.AddDays(5).AddHours(21).AddMinutes(30),
                Author=context.Users.First(),
            });
            context.Tasks.Add(new Task()
            {
                Title="Comming Tasks <Anonymous>",
                StartDateTime=DateTime.Now.Date.AddDays(10).AddHours(40).AddMinutes(20),
                Author=context.Users.Find(),
            });
            context.Tasks.Add(new Task()
            {
                Title="FMI Mahthematics Exam",
                StartDateTime=DateTime.Now.Date.AddDays(-1).AddHours(5).AddMinutes(25),
                Duration=TimeSpan.FromHours(1.2),
                Comments=new HashSet<Comment>()
                {
                    new Comment() {Text="<Anonymous> comment" },
                    new Comment() {Text="User comment",Author=context.Users.First() }
                }
            });
            context.Tasks.Add(new Task()
            {
                Title = "Passed Tasks <Anonymous>",
                StartDateTime = DateTime.Now.Date.AddDays(-2).AddHours(10).AddMinutes(30),
                Duration = TimeSpan.FromHours(1.5),
                Comments = new HashSet<Comment>()
                {
                    new Comment() {Text="<Anonymous> comment" },
                    new Comment() {Text="User comment",Author=context.Users.First() }
                }
            });

            context.Tasks.Add(new Task()
            {
                Title = "ASP.NET MVC Lab",
                StartDateTime = DateTime.Now.Date.AddDays(3).AddHours(11).AddMinutes(30),
                Author = context.Users.First(),
                Duration = TimeSpan.FromHours(2),
                Location = "FMI",
                Comments = new HashSet<Comment>() {
                    new Comment() { Text = "<Anonymous> comment" },
                    new Comment() { Text = "User comment", Author = context.Users.First() },
                    new Comment() { Text = "Another <user> comment", Author = context.Users.First() }
                }
            });

            context.SaveChanges();
        }
        



        //protected override void Seed(ApplicationDbContext context)
        //{
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        //}
    }
}
