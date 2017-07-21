using BAPA_LMS.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BAPA_LMS.Models.ActivityViewModels
{
	public class ActivityDeleteViewModel
	{
		public string Name { get; set; }

		public static implicit operator ActivityDeleteViewModel(Activity model)
		{
			return new ActivityDeleteViewModel
			{
				Name = model.Name
			};
		}
	}
}