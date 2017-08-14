using BAPA_LMS.Models.DB;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BAPA_LMS.Models.CourseViewModels
{
    public class CourseListViewModel
    {
        public CourseListRow[] Courses { get; set; }

        [Display(Name = "Sökord:")]
        public string Filter { get; set; }

        public string StartRange { get; set; }

        public string EndRange { get; set; }

        public string SortParam { get; set; }

        public int Count { get; set; }

        public int Offset { get; set; }

        public const int PageSize = 10;
    }

	public class CourseListRow
	{
		public int Id { get; set; }

		[Display(Name = "Namn")]
		public string Name { get; set; }

		[Display(Name = "Beskrivning")]
		public string Description { get; set; }

		[Display(Name = "Startdatum")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime StartDate { get; set; }

	//	public static implicit operator CourseListViewModel(Course model)
	//	{
	//		return new CourseListViewModel
	//		{
	//			Id = model.Id,
	//			Name = model.Name,
	//			Description = model.Description,
	//			StartDate = model.StartDate,
	//		};

	//	}
	}
}