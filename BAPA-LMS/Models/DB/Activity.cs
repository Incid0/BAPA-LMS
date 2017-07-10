using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BAPA_LMS.Models.DB
{
	public enum ActivityTypes {
		[Description("E-Learning")]
		ELearning,
		Föreläsning,
		Inlämningsuppgift,
		Övningstillfälle,
		Annat
	}

	public class Activity
	{
		public int Id { get; set; }
		public ActivityTypes Type { get; set; }

		[StringLength(40)]
		public string Name { get; set; }

		[StringLength(2000)]
		public string Description { get; set; }
		public DateTime StartTime { get; set; }
		public DateTime EndTime { get; set; }
        public int ModuleId { get; set; }

        public virtual Module Module { get; set; }
	}
}