using BAPA_LMS.Models.DB;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BAPA_LMS.Models.CourseViewModels
{
	public class CourseEditViewModel
	{
        public int Id { get; set; }

        [Display(Name = "Namn")]
        public string Name { get; set; }

        [Display(Name = "Beskrivning")]
        public string Description { get; set; }

        [Display(Name = "Startdatum")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime StartDate { get; set; }

        public static implicit operator CourseEditViewModel(Course model)
		{
			return new CourseEditViewModel
			{
                Id = model.Id,
                Name = model.Name,
                Description = model.Description,    
                StartDate = model.StartDate
			};

		}
	}
}