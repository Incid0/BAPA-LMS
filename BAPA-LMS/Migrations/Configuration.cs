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
	using System.Text;

	internal sealed class Configuration : DbMigrationsConfiguration<BAPA_LMS.DataAccessLayer.LMSDbContext>
	{
		public Configuration()
		{
			AutomaticMigrationsEnabled = false;
		}

		protected override void Seed(DataAccessLayer.LMSDbContext context)
		{
			Random rng = new Random();
			int i;

			try
			{
				context.Configuration.AutoDetectChangesEnabled = false;

				context.ActivityTypes.AddOrUpdate(
					new ActivityType { Id = 1, Name = "E-Learning", Color = "DeepPink", Submit = false },
					new ActivityType { Id = 2, Name = "Föreläsning", Color = "MediumTurquoise", Submit = false },
					new ActivityType { Id = 3, Name = "Inlämningsuppgift", Color = "Orange", Submit = true },
					new ActivityType { Id = 4, Name = "Övningstillfälle", Color = "DarkBlue", Submit = false },
					new ActivityType { Id = 5, Name = "Annat", Color = "Purple", Submit = false });
				context.SaveChanges();

				context.Courses.AddOrUpdate(
					new Course
					{
						Id = 1,
						Name = "Lexicons Monsterkurs",
						Description = "Grundläggande inom .NET\nLorem ipsum dolor sit amet, consectetur adipiscing elit. Nullam vel sem mauris. Mauris vulputate pharetra facilisis. Mauris aliquam sed massa vel mollis. Morbi finibus tortor ultrices enim volutpat, et semper purus dictum. Fusce tellus purus, pulvinar id ornare in, vehicula sit amet magna. Fusce et vestibulum ante. Sed in commodo mi, eu maximus ligula. Sed a libero ultrices, volutpat velit ac, lacinia ipsum. Cras dictum lacinia ante, dapibus tincidunt lectus faucibus et. Donec eleifend, mi eget ultricies pretium, arcu diam sodales lectus, at tincidunt dui felis facilisis sapien. Sed in lorem viverra, vulputate lectus ac, sollicitudin velit. Sed nec est sed massa blandit hendrerit at vestibulum nisi.Suspendisse felis sem,consequat et leo a,viverra pharetra quam.Donec in urna ut risus venenatis finibus.In orci tortor,pretium eget maximus iaculis,porttitor a turpis.Donec id aliquam nulla,ac euismod est.Curabitur euismod odio ut dui fermentum,feugiat dignissim dui gravida.Maecenas aliquam vestibulum accumsan.In luctus erat nec nunc pellentesque tristique.Integer et pellentesque lectus.Vestibulum et sapien a neque mattis scelerisque et in sem.Nam et porta nunc,at accumsan justo.Praesent in odio ex.Pellentesque pretium,magna ut auctor congue,libero tellus suscipit nisl,quis dapibus nisl nulla vitae leo.Mauris dictum nisl nec maximus lobortis.Cras suscipit sem ut felis dignissim suscipit.Aliquam consectetur,justo ac molestie tempor,mi urna tempus augue,sed finibus turpis mauris ut eros.",
						StartDate = DateTime.Parse("2017/07/10")
					},
					new Course
					{
						Id = 2,
						Name = "Rekursiva metoder med John",
						Description = "Lär dig programmera som en gud med Johns tips och tricks\nLorem ipsum dolor sit amet, consectetur adipiscing elit. Nullam vel sem mauris. Mauris vulputate pharetra facilisis. Mauris aliquam sed massa vel mollis. Morbi finibus tortor ultrices enim volutpat, et semper purus dictum. Fusce tellus purus, pulvinar id ornare in, vehicula sit amet magna. Fusce et vestibulum ante. Sed in commodo mi, eu maximus ligula. Sed a libero ultrices, volutpat velit ac, lacinia ipsum. Cras dictum lacinia ante, dapibus tincidunt lectus faucibus et. Donec eleifend, mi eget ultricies pretium, arcu diam sodales lectus, at tincidunt dui felis facilisis sapien. Sed in lorem viverra, vulputate lectus ac, sollicitudin velit. Sed nec est sed massa blandit hendrerit at vestibulum nisi.Suspendisse felis sem,consequat et leo a,viverra pharetra quam.Donec in urna ut risus venenatis finibus.In orci tortor,pretium eget maximus iaculis,porttitor a turpis.Donec id aliquam nulla,ac euismod est.Curabitur euismod odio ut dui fermentum,feugiat dignissim dui gravida.Maecenas aliquam vestibulum accumsan.In luctus erat nec nunc pellentesque tristique.Integer et pellentesque lectus.Vestibulum et sapien a neque mattis scelerisque et in sem.Nam et porta nunc,at accumsan justo.Praesent in odio ex.Pellentesque pretium,magna ut auctor congue,libero tellus suscipit nisl,quis dapibus nisl nulla vitae leo.Mauris dictum nisl nec maximus lobortis.Cras suscipit sem ut felis dignissim suscipit.Aliquam consectetur,justo ac molestie tempor,mi urna tempus augue,sed finibus turpis mauris ut eros.",
						StartDate = DateTime.Parse("2017/07/10")

					}
					);
				context.SaveChanges();
				context.Modules.AddOrUpdate(
					new Module
					{
						Id = 1,
						Name = "C# Basic",
						Description = "Grundläggande inom C#",
						CourseId = 1,
						StartDate = DateTime.Parse("2017/07/10"),
						EndDate = DateTime.Parse("2017/07/28")
					},

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

				DateTime flow = DateTime.Today - new TimeSpan(4, 0, 0, 0);
				var bla = new StringBuilder("Bla ", 1000);

				for (i = 0; i < 200; i++)
				{
					bla.Append("bla ");
				}

				i = 1;
				while (i < 30)
				{
					if (flow.DayOfWeek != DayOfWeek.Saturday && flow.DayOfWeek != DayOfWeek.Sunday)
					{
						if (rng.Next(4) > 0)
						{
							context.Activities.AddOrUpdate(
								new Activity
								{
									Id = i++,
									Name = "Förmiddagsaktivitet " + i,
									TypeId = 1 + rng.Next(5),
									ModuleId = 1 + rng.Next(4),
									Description = bla.ToString(),
									StartTime = flow + new TimeSpan(0, 8, 30, 0),
									EndTime = flow + new TimeSpan(0, 12, 0, 0)
								});
						}
						if (rng.Next(4) > 0)
						{
							context.Activities.AddOrUpdate(
								new Activity
								{
									Id = i++,
									Name = "Eftermiddagsaktivitet " + i,
									TypeId = 1 + rng.Next(5),
									ModuleId = 1 + rng.Next(4),
									Description = bla.ToString(),
									StartTime = flow + new TimeSpan(0, 13, 0, 0),
									EndTime = flow + new TimeSpan(0, 17, 0, 0)
								});
						}
					}
					flow += new TimeSpan(1, 0, 0, 0);
				}
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
				i = 0;

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
						if (i > 0)
						{
							if (i < 6)
							{
								user.CourseId = 1;
							}
							else
							{
								user.CourseId = 2;
							}
						}
						var result = userManager.Create(user, "foobar");
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

                //string[] courseNames = new[] { "Basic", "Pascal", "Cobol", "C++", "C#", "Javascript", "Html5", "Python", "PHP", "Ruby", "med John", "med Dimitris", "Grund", ".Net", "Classic", "FullStack" };
                //string[] courseHashes = new[] { "#nybörjare", "#medelsvår", "#avancerad", "#1337h4xx0r", "#historik", "#teori", "#praktik", "#e-learning", "#föreläsning", "#verktyg", "#projekt", "#grupparbete", "#nivåer" };
                //string[] courseCities = new[] { "Stockholm", "Göteborg", "Malmö", "Linköping", "Kiruna", "Östersund", "Hudiksvall", "Jönköping", "Uppsala", "Västerås", "Sundsvall", "Gävle", "Umeå" };
                //for (i = 3; i < 20000; i++)
                //{
                //    context.Courses.AddOrUpdate(
                //        new Course
                //        {
                //            Id = i,
                //            Name = string.Format("{0} {1}", courseNames[rng.Next(10)], courseNames[10 + rng.Next(6)]),
                //            Description = string.Format("Ort: {0}\nNyckelord: {1} {2} {3}",
                //                courseCities[rng.Next(courseCities.Length)],
                //                courseHashes[rng.Next(courseHashes.Length)],
                //                courseHashes[rng.Next(courseHashes.Length)],
                //                courseHashes[rng.Next(courseHashes.Length)]),
                //            StartDate = DateTime.Today - new TimeSpan(365, 0, 0, 0) + new TimeSpan(rng.Next(365 * 3), 0, 0, 0)
                //        });
                //    if (i % 100 == 0) context.SaveChanges();
                //}
                //context.SaveChanges();
            }
			finally
			{
				context.Configuration.AutoDetectChangesEnabled = true;
			}
		}
	}

}