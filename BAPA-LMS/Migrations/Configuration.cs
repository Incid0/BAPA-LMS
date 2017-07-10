namespace BAPA_LMS.Migrations
{
    using BAPA_LMS.Models.DB;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using BAPA_LMS.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<BAPA_LMS.DataAccessLayer.LMSDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(DataAccessLayer.LMSDbContext context)
        {
            
            context.Courses.AddOrUpdate( c => c.Id,
                new Course {                    
                    Name = "Lexicons Monsterkurs",
                    Description = "Grundl�ggande inom .NET",
                    StartDate = DateTime.Parse("2017/07/10") }
                
                );
            var courseId = context.Courses.Find(1);
            context.Modules.AddOrUpdate(c => c.Id,
                new Module {
                    Name = "C# Basic",
                    Description = "Grundl�ggande inom C#",
                    Course = courseId,
                    StartDate = DateTime.Parse("2017/07/10"),
                    EndDate = DateTime.Parse("2017/07/28") }

                );
            var moduleId = context.Modules.Find(1);
            context.Activities.AddOrUpdate( c => c.Id,
                new Activity {
                    Name = "Aktivitet",
                    Type = ActivityTypes.Annat,
                    Module = moduleId,
                    Description = "Generell aktivitetsinfo",
                    StartTime = DateTime.Parse("2017/07/10"),
                    EndTime = DateTime.Parse("2017/07/28") },
                new Activity {
                    Name = "En till Aktivitet!",
                    Type = ActivityTypes.Annat,
                    Module = moduleId,
                    Description = "Generell aktivitetsinfo",
                    StartTime = DateTime.Parse("2017/07/10"),
                    EndTime = DateTime.Parse("2017/07/28")
                },
                
                new Activity {
                    Name = "Annan Aktivitet",
                    Type = ActivityTypes.Annat,
                    Module = moduleId,
                    Description = "Generell aktivitetsinfo",
                    StartTime = DateTime.Parse("2017/07/10"),
                    EndTime = DateTime.Parse("2017/07/28")
                
                }

                );

           

        


            RoleStore<IdentityRole> roleStore = new RoleStore<IdentityRole>(context);
            RoleManager<IdentityRole> roleManager = new RoleManager<IdentityRole>(roleStore);

            string[] roleNames = new[] { "Admin", "Member" };
            foreach (string roleName in roleNames)
            {
                if (!context.Roles.Any(r => r.Name == roleName))
                {
                    IdentityRole identityRole = new IdentityRole { Name = roleName };
                    IdentityResult result = roleManager.Create(identityRole);
                    if (!result.Succeeded)
                    {
                        throw new Exception(string.Join("\n", result.Errors));
                    }
                }
            }
            UserStore<ApplicationUser> userStore = new UserStore<ApplicationUser>(context);
            UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(userStore);
            string[] emails = new[] { "admin@bapa.se", "elev1@bapa.se", "elev2@bapa.se", "elev3@bapa.se", "elev4@bapa.se" };
            string[] firstName = new[] { "John", "Erik", "Anders", "Basse", "Olga" };
            string[] lastName = new[] { "Hellman", "Svensson", "Eriksson", "Nybom", "Stanislav" };
            int i = 0;

            foreach (string email in emails)
            {
                if (!context.Users.Any(u => u.UserName == email))
                {
                    ApplicationUser user = new ApplicationUser { UserName = email, Email = email, FirstName = firstName[i], LastName = lastName[i]};
                    var result = userManager.Create(user,"foobar");
                    if (!result.Succeeded)
                    {
                        throw new Exception(string.Join("\n", result.Errors));
                    }
                }
                i++;
            }
            ApplicationUser adminUser = userManager.FindByName("admin@bapa.se");
            userManager.AddToRole(adminUser.Id, "Admin");

         

            foreach (ApplicationUser user in userManager.Users.ToList().Where(u => u.Email != "admin@bapa.se"))
            {
                userManager.AddToRole(user.Id, "Member");
            }
        }
    }
    
}