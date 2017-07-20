using BAPA_LMS.Models.DB;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BAPA_LMS.Models.ModuleViewModels
{
    public class ModuleEditViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(40)]
        [Display(Name = "Namn")]
        public string Name { get; set; }

        [Required]
        [StringLength(2000)]
        [Display(Name = "Beskrivning")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Startdatum")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        [Required]
        [Display(Name = "Slutdatum")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }

        public DateTime CourseStartDate { get; set; }

		public ModuleEditViewModel()
		{
			StartDate = DateTime.Today;
			EndDate = DateTime.Today;
		}

		public static implicit operator ModuleEditViewModel(Module model)
        {
            return new ModuleEditViewModel
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description,
                StartDate = model.StartDate,
                EndDate = model.EndDate
            };

        }
    }
}