using BAPA_LMS.Models.DB;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace BAPA_LMS.DataAccessLayer
{
	public class LMSDbContext : DbContext
	{
		public LMSDbContext(): base("DefaultConnection") { }

		public DbSet<Course> Courses { get; set; }
		public DbSet<Module> Modules { get; set; }
		public DbSet<Activity> Activities { get; set; }
	}

}