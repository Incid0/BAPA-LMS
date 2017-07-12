using BAPA_LMS.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BAPA_LMS.Models.ActivityViewModels
{
    public class ActivityEditViewModel
    {
        public static implicit operator ActivityEditViewModel(Activity model)
        {
            return new ActivityEditViewModel
            {

            };

        }
    }
}