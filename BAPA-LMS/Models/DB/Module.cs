using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BAPA_LMS.Models.DB
{
	public class Module
	{
		public int Id { get; set; }

		[StringLength(40)]
		public string Name { get; set; }

		[StringLength(2000)]
		public string Description { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }

		public virtual Course Course { get; set; }
		public virtual ICollection<Activity> Activities { get; set; }
	}
}