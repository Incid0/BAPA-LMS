using BAPA_LMS.Models.DB;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BAPA_LMS.Models.CourseViewModels
{
	public class CourseDetailViewModel
	{
		public int Id { get; set; }

		[Display(Name = "Namn")]
		public string Name { get; set; }

		[Display(Name = "Beskrivning")]
		public string Description { get; set; }

		[Display(Name = "Startdatum")]
		public DateTime StartDate { get; set; }

		[Display(Name = "Modullista")]
		public ICollection<Module> Modules { get; set; }
        public ICollection<ApplicationUser> Members { get; set; }
		public static implicit operator CourseDetailViewModel(Course model)
		{
			return new CourseDetailViewModel
			{
				Name = model.Name,
				Description = model.Description,
				StartDate = model.StartDate,
				Modules = model.Modules,
                Members = model.Members
			};

		}
	}
}