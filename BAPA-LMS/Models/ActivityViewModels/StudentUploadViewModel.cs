using BAPA_LMS.Models.DB;
using System;
using System.ComponentModel.DataAnnotations;

namespace BAPA_LMS.Models.ActivityViewModels
{
    public class StudentUploadViewModel
    {
        [Key]
        public int Id { get; set; }
        public string Email { get; set; }
        public string UserId { get; set; }
        public int CourseId { get; set; }
        public int ActivityId { get; set; }

        public string UserdId { get; set; }
        public static implicit operator StudentUploadViewModel(FileDocument model)
        {
            return new StudentUploadViewModel
            {
                Id = model.Id,
                Email = model.Member.Email,
                UserId = model.Member.Id,
                CourseId = model.Course.Id,
                ActivityId = model.Activity.Id
            };

        }
    }
}