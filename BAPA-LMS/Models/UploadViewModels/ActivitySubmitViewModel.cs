using BAPA_LMS.Models.DB;
using System;
using System.ComponentModel.DataAnnotations;

namespace BAPA_LMS.Models.ActivityViewModels
{
    public class ActivitySubmitViewModel
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Namn")]
        public string Name { get; set; }

        [Display(Name = "Beskrivning")]
        public string Description { get; set; }

        [Display(Name = "Slutdatum")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime EndDate { get; set; }

        [Display(Name = "Sluttid")]
        [DisplayFormat(DataFormatString = "{0:HH:mm}")]
        public DateTime EndTime { get; set; }
        [Display(Name = "Typ av aktivitet")]
        
        public ActivityType Type { get; set; }

        public static implicit operator ActivitySubmitViewModel(Activity model)
        {
            return new ActivitySubmitViewModel
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description,  
                EndDate = model.EndTime,
                EndTime = model.EndTime,
                Type = model.Type
                
            };
        }
    }
}