using BAPA_LMS.Models.DB;
using System;
using System.ComponentModel.DataAnnotations;

namespace BAPA_LMS.Models.ActivityViewModels
{
    public class ActivityUploadViewModel
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Namn")]
        public string Name { get; set; }

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
        public bool FileSubmitted { get; set; }

        public static implicit operator ActivityUploadViewModel(Activity model)
        {
            return new ActivityUploadViewModel
            {
                Id = model.Id,
                Name = model.Name,
                EndDate = model.EndTime,
                EndTime = model.EndTime,
                Type = model.Type,
                ModuleName = model.Module.Name,
                FileSubmitted = model.DocumentIsUploaded
            };

        }
    }
}