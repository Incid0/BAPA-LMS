using BAPA_LMS.Models.DB;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BAPA_LMS.Models.ActivityViewModels
{
    public class ActivityDetailViewModel
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Namn")]
        public string Name { get; set; }

        [Display(Name = "Beskrivning")]
        public string Description { get; set; }

        [Display(Name = "Startdatum")]
		[DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
		public DateTime StartDate { get; set; }

		[Display(Name = "Starttid")]
		[DisplayFormat(DataFormatString = "{0:HH:mm}")]
		public DateTime StartTime { get; set; }

		[Display(Name = "Slutdatum")]
		[DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
		public DateTime EndDate { get; set; }

		[Display(Name = "Sluttid")]
		[DisplayFormat(DataFormatString = "{0:HH:mm}")]
		public DateTime EndTime { get; set; }

        [Display(Name = "Typ av aktivitet")]
        public ActivityType Type { get; set; }

		[Display(Name = "Ingår i modul")]
		public string ModuleName { get; set; }

		public static implicit operator ActivityDetailViewModel(Activity model)
        {
            return new ActivityDetailViewModel
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description,
				StartDate = model.StartTime,
				StartTime = model.StartTime,
				EndDate = model.EndTime,
				EndTime = model.EndTime,
                Type = model.Type,
				ModuleName = model.Module.Name
            };

        }
    }
}