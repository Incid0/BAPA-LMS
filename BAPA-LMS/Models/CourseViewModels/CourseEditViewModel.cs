using BAPA_LMS.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BAPA_LMS.Models.CourseViewModels
{
	public class CourseEditViewModel
	{
		public static implicit operator CourseEditViewModel(Course model)
		{
			return new CourseEditViewModel
			{

			};

		}
	}
}