using BAPA_LMS.Models.DB;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BAPA_LMS.Models.UserViewModels
{
    public class DeleteUserViewModel
    {
        [Key]
        public string Id { get; set; }
        public string Email { get; set; }
        public string CourseName { get; set; }
        public string FullName { get; set; }
        public int CourseId { get; set; }



        public static implicit operator DeleteUserViewModel(ApplicationUser model)
        {
            return new DeleteUserViewModel
            {
                Id = model.Id,
                Email = model.Email,
                CourseName = model.Course.Name,
                FullName = model.FullName,
                CourseId = model.Course.Id
           
            };

        }  
    

    }
}