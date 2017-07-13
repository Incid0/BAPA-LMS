using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BAPA_LMS.Models.DB
{
	public class ActivityType
	{
		public int Id { get; set; }

		[StringLength(40)]
		public string Name { get; set; }

		[StringLength(20)]
		public string Color{ get; set; }
		public bool Submit { get; set; }
	}
}