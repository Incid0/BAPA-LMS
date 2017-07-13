using BAPA_LMS.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BAPA_LMS.Models.CourseViewModels
{
    public class CourseIndexViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public static implicit operator CourseIndexViewModel(Course model)
        {
            return new CourseIndexViewModel
            {
                Name = model.Name
            };
        }
    }
}