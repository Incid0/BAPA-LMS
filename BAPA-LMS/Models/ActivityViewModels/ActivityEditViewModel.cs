using BAPA_LMS.Models.DB;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BAPA_LMS.Models.ActivityViewModels
{
    public class ActivityEditViewModel
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

        [Required]
        [RegularExpression(@"^(0[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$", ErrorMessage = "Fel tidsformat (hh:mm)")]
        [Display(Name = "Starttid")]
        public string StartTime { get; set; }

        [Required]
        [RegularExpression(@"^(0[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$", ErrorMessage = "Fel tidsformat (hh:mm)")]
        [Display(Name = "Sluttid")]        
        public string EndTime { get; set; }
        
        [Required]
        [Display(Name = "Typ av aktivitet")]
        public int Type { get; set; }

        public List<ActivityType> Types { get; set; }

        public ActivityEditViewModel()
        {
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;
        }

        public static implicit operator ActivityEditViewModel(Activity model)
        {
            return new ActivityEditViewModel
            {
                Id = model.Id,
				Name = model.Name,
                Description = model.Description,
                StartDate = model.StartTime,
                StartTime = model.StartTime.ToString("HH:mm"),
                EndDate = model.EndTime,
                EndTime = model.EndTime.ToString("HH:mm"),
                Type = model.TypeId
            };

        }
    }
}