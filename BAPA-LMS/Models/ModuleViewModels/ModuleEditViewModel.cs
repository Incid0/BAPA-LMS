using BAPA_LMS.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BAPA_LMS.Models.ModuleViewModels
{
    public class ModuleEditViewModel
    {
        public static implicit operator ModuleEditViewModel(Module model)
        {
            return new ModuleEditViewModel
            {

            };

        }
    }
}