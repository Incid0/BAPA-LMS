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
            
            context.Courses.AddOrUpdate(
                new Course
                {
                    Id = 1,
                    Name = "Lexicons Monsterkurs",
                    Description = "Grundläggande inom .NET",
                    StartDate = DateTime.Parse("2017/07/10")
                },
                
                
                new Course
                {
                    Id = 2,
                    Name = "Rekursiva metoder med John",
                    Description = "Lär dig programmera som en gud med Johns tips och tricks",
                    StartDate = DateTime.Parse("2017/07/10")

                }
                );
            context.SaveChanges();
            context.Modules.AddOrUpdate( 
                new Module {
                    Id = 1,
                    Name = "C# Basic",
                    Description = "Grundläggande inom C#",
                    CourseId = 1,
                    StartDate = DateTime.Parse("2017/07/10"),
                    EndDate = DateTime.Parse("2017/07/28") },

                new Module
                {
                    Id = 2,
                    Name = "Java",
                    Description = "Grundläggande inom Java",
                    CourseId = 1,
                    StartDate = DateTime.Parse("2017/08/10"),
                    EndDate = DateTime.Parse("2017/08/28")
                },

                new Module
                {
                    Id = 3,
                    Name = "AngularJS",
                    Description = "Grundläggande inom AngularJS",
                    CourseId = 1,
                    StartDate = DateTime.Parse("2017/09/10"),
                    EndDate = DateTime.Parse("2017/09/28")
                
                },
                 new Module
                 {
                     Id = 4,
                     Name = "PHP Hello World",
                     Description = "Världens bästa kodspråk maximeras till fullo",
                     CourseId = 2,
                     StartDate = DateTime.Parse("2017/07/10"),
                     EndDate = DateTime.Parse("2017/07/28")
                 },

                new Module
                {
                    Id = 5,
                    Name = "Konspirationsteorier",
                    Description = "Hur man enklast och smidigast tillverkar en folie hatt av aluminium",
                    CourseId = 2,
                    StartDate = DateTime.Parse("2017/08/10"),
                    EndDate = DateTime.Parse("2017/08/28")
                },

                new Module
                {
                    Id = 6,
                    Name = "Pekpinnefäktning",
                    Description = "Lär dig prygla till elever som inte förtjänar annat",
                    CourseId = 2,
                    StartDate = DateTime.Parse("2017/09/10"),
                    EndDate = DateTime.Parse("2017/09/28")

                }
                );
            context.SaveChanges();
            context.Activities.AddOrUpdate(
                new Activity {
                    Id = 1,
                    Name = "Aktivitet",
                    Type = ActivityTypes.Annat,
                    ModuleId = 1,
                    Description = "Generell aktivitetsinfo",
                    StartTime = DateTime.Parse("2017/07/10"),
                    EndTime = DateTime.Parse("2017/07/10") },
                new Activity {
                    Id = 2,
                    Name = "En till Aktivitet!",
                    Type = ActivityTypes.Annat,
                    ModuleId = 1,
                    Description = "Generell aktivitetsinfo",
                    StartTime = DateTime.Parse("2017/07/11"),
                    EndTime = DateTime.Parse("2017/07/11")
                },
                
                new Activity {
                    Id = 3,
                    Name = "Annan Aktivitet",
                    Type = ActivityTypes.Annat,
                    ModuleId = 1,
                    Description = "Generell aktivitetsinfo",
                    StartTime = DateTime.Parse("2017/07/12"),
                    EndTime = DateTime.Parse("2017/07/12")
                
                }, new Activity
                {
                    Id = 4,
                    Name = "Aktivitet",
                    Type = ActivityTypes.Annat,
                    ModuleId = 2,
                    Description = "Generell aktivitetsinfo",
                    StartTime = DateTime.Parse("2017/07/13"),
                    EndTime = DateTime.Parse("2017/07/13")
                },
                new Activity
                {
                    Id = 5,
                    Name = "En till Aktivitet!",
                    Type = ActivityTypes.Annat,
                    ModuleId = 2,
                    Description = "Generell aktivitetsinfo",
                    StartTime = DateTime.Parse("2017/07/14"),
                    EndTime = DateTime.Parse("2017/07/14")
                },

                new Activity
                {
                    Id = 6,
                    Name = "Annan Aktivitet",
                    Type = ActivityTypes.Annat,
                    ModuleId = 3,
                    Description = "Generell aktivitetsinfo",
                    StartTime = DateTime.Parse("2017/07/15"),
                    EndTime = DateTime.Parse("2017/07/15")

                },
                new Activity
                {
                    Id = 7,
                    Name = "Aktivitet",
                    Type = ActivityTypes.Annat,
                    ModuleId = 4,
                    Description = "Generell aktivitetsinfo",
                    StartTime = DateTime.Parse("2017/07/16"),
                    EndTime = DateTime.Parse("2017/07/16")
                },
                new Activity
                {
                    Id = 8,
                    Name = "En till Aktivitet!",
                    Type = ActivityTypes.Annat,
                    ModuleId = 5,
                    Description = "Generell aktivitetsinfo",
                    StartTime = DateTime.Parse("2017/07/17"),
                    EndTime = DateTime.Parse("2017/07/17")
                },

                new Activity
                {
                    Id = 9,
                    Name = "Annan Aktivitet",
                    Type = ActivityTypes.Annat,
                    ModuleId = 6,
                    Description = "Generell aktivitetsinfo",
                    StartTime = DateTime.Parse("2017/07/18"),
                    EndTime = DateTime.Parse("2017/07/18")

                },
                new Activity
                {
                    Id = 10,
                    Name = "Aktivitet",
                    Type = ActivityTypes.Annat,
                    ModuleId = 5,
                    Description = "Generell aktivitetsinfo",
                    StartTime = DateTime.Parse("2017/07/19"),
                    EndTime = DateTime.Parse("2017/07/19")
                },
                new Activity
                {
                    Id = 11,
                    Name = "En till Aktivitet!",
                    Type = ActivityTypes.Annat,
                    ModuleId = 6,
                    Description = "Generell aktivitetsinfo",
                    StartTime = DateTime.Parse("2017/07/20"),
                    EndTime = DateTime.Parse("2017/07/20")
                },

                new Activity
                {
                    Id = 12,
                    Name = "Annan Aktivitet",
                    Type = ActivityTypes.Annat,
                    ModuleId = 6,
                    Description = "Generell aktivitetsinfo",
                    StartTime = DateTime.Parse("2017/07/21"),
                    EndTime = DateTime.Parse("2017/07/21")

                }

                );

            context.SaveChanges();




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
            string[] emails = new[] { "admin@bapa.se", "elev1@bapa.se", "elev2@bapa.se", "elev3@bapa.se", "elev4@bapa.se", "elev5@bapa.se", "elev6@bapa.se", "elev7@bapa.se", "elev8@bapa.se" };
            string[] firstName = new[] { "John", "Erik", "Anders", "Basse", "Olga", "Anton", "Andreas", "Peter", "Lasse", "Mats" };
            string[] lastName = new[] { "Hellman", "Svensson", "Eriksson", "Nybom", "Stanislav", "Bernadotte", "Karlsson", "Larsson", "Persson", "Keisanen" };
            int i = 0;
           
            foreach (string email in emails)
            {
                if (!context.Users.Any(u => u.UserName == email))
                {
                   
                    ApplicationUser user = new ApplicationUser
                    {
                        UserName = email,
                        Email = email,
                        FirstName = firstName[i],
                        LastName = lastName[i],

                     
                        
                  
                    };
                   if(i < 5)
                    {
                        user.CourseId = 1;
                    }
                    else
                    {
                        user.CourseId = 2;
                    }
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
            context.SaveChanges();
        }
    }
    
}