using BAPA_LMS.Models.DB;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BAPA_LMS.Models.ModuleViewModels
{
    public class ModuleDetailViewModel
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

		[Display(Name = "Slutdatum")]
		[DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime EndDate { get; set; }

        [Display(Name = "Aktivitetslista")]
        public ICollection<Activity> Activities { get; set; }

        public static implicit operator ModuleDetailViewModel(Module model)
        {
            return new ModuleDetailViewModel
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                Activities = model.Activities
            };

        }
    }
}