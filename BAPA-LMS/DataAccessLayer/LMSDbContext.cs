using BAPA_LMS.Models.DB;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace BAPA_LMS.DataAccessLayer
{
	public class LMSDbContext : IdentityDbContext<ApplicationUser>
	{
		public LMSDbContext(): base("DefaultConnection", throwIfV1Schema: false) { }

		public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
		{
		public DbSet<Course> Courses { get; set; }
		public DbSet<Module> Modules { get; set; }
		public DbSet<Activity> Activities { get; set; }

		public static LMSDbContext Create()
		{
			return new LMSDbContext();
		}
	}
}