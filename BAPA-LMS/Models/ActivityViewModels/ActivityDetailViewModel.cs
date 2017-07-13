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
        public DateTime StartTime { get; set; }

        [Display(Name = "Slutdatum")]
        public DateTime EndTime { get; set; }

        [Display(Name = "Aktivitet")]
        public ActivityType Type { get; set; }

        public static implicit operator ActivityDetailViewModel(Activity model)
        {
            return new ActivityDetailViewModel
            {
                Name = model.Name,
                Description = model.Description,
                StartTime = model.StartTime,
                EndTime = model.EndTime,
                Type = model.Type
            };

        }
    }
}