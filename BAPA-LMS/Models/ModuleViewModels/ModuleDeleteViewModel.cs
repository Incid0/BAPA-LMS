using BAPA_LMS.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BAPA_LMS.Models.ModuleViewModels
{
	public class ModuleDeleteViewModel
	{
		public string Name { get; set; }

		public static implicit operator ModuleDeleteViewModel(Module model)
		{
			return new ModuleDeleteViewModel
			{
				Name = model.Name
			};
		}
	}
}