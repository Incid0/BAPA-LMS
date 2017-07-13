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

			context.ActivityTypes.AddOrUpdate(
				new ActivityType { Id = 1, Name = "E-Learning", Color = "DarkRed", Submit = false },
				new ActivityType { Id = 2, Name = "F�rel�sning", Color = "DarkGreen", Submit = false },
				new ActivityType { Id = 3, Name = "Inl�mningsuppgift", Color = "Orange", Submit = true },
				new ActivityType { Id = 4, Name = "�vningstillf�lle", Color = "DarkBlue", Submit = false },
				new ActivityType { Id = 5, Name = "Annat", Color = "Purple", Submit = false });
			context.SaveChanges();

			context.Courses.AddOrUpdate(
				new Course
				{
					Id = 1,
					Name = "Lexicons Monsterkurs",
					Description = "Grundl�ggande inom .NET",
					StartDate = DateTime.Parse("2017/07/10")
				},


				new Course
				{
					Id = 2,
					Name = "Rekursiva metoder med John",
					Description = "L�r dig programmera som en gud med Johns tips och tricks",
					StartDate = DateTime.Parse("2017/07/10")

				}
				);
			context.SaveChanges();
			context.Modules.AddOrUpdate(
				new Module
				{
					Id = 1,
					Name = "C# Basic",
					Description = "Grundl�ggande inom C#",
					CourseId = 1,
					StartDate = DateTime.Parse("2017/07/10"),
					EndDate = DateTime.Parse("2017/07/28")
				},

				new Module
				{
					Id = 2,
					Name = "Java",
					Description = "Grundl�ggande inom Java",
					CourseId = 1,
					StartDate = DateTime.Parse("2017/08/10"),
					EndDate = DateTime.Parse("2017/08/28")
				},

				new Module
				{
					Id = 3,
					Name = "AngularJS",
					Description = "Grundl�ggande inom AngularJS",
					CourseId = 1,
					StartDate = DateTime.Parse("2017/09/10"),
					EndDate = DateTime.Parse("2017/09/28")

				},
				 new Module
				 {
					 Id = 4,
					 Name = "PHP Hello World",
					 Description = "V�rldens b�sta kodspr�k maximeras till fullo",
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
					Name = "Pekpinnef�ktning",
					Description = "L�r dig prygla till elever som inte f�rtj�nar annat",
					CourseId = 2,
					StartDate = DateTime.Parse("2017/09/10"),
					EndDate = DateTime.Parse("2017/09/28")

				}
				);
			context.SaveChanges();

			DateTime flow = DateTime.Today - new TimeSpan(1, 0, 0, 0);
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
								Name = "F�rmiddagsaktivitet " + i,
								TypeId = 1 + rng.Next(5),
								ModuleId = 1,
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
								ModuleId = 1,
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
					if (i < 5)
					{
						user.CourseId = 1;
					}
					else
					{
						user.CourseId = 2;
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
		}
	}

}