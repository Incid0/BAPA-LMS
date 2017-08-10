using BAPA_LMS.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BAPA_LMS.Models.CourseViewModels
{
    public class CourseDeleteViewModel
    {
        public string Name { get; set; }
        public static implicit operator CourseDeleteViewModel(Course model)
        {
            return new CourseDeleteViewModel
            {
				Name = model.Name
            };
        }
    }
}