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
        [Display(Name = "Starttid")]
        [DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime StartTime { get; set; }

        [Required]
        [Display(Name = "Slutdatum")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }

        [Required]
        [Display(Name = "Sluttid")]
        [DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime EndTime { get; set; }

        [Required]
        [Display(Name = "Typ av aktivitet")]
        public ActivityType Type { get; set; }

        public static implicit operator ActivityEditViewModel(Activity model)
        {
            return new ActivityEditViewModel
            {
                Id = model.Id,
                Description = model.Description,
                StartDate = model.StartTime,
                StartTime = model.StartTime,
                EndDate = model.EndTime,
                EndTime = model.EndTime,
                Type = model.Type
            };

        }
    }
}