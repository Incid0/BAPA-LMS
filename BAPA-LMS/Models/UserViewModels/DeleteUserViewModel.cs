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
        public string FullName { get; set; }

        public static implicit operator DeleteUserViewModel(ApplicationUser model)
        {
            return new DeleteUserViewModel
            {
                FullName = model.FullName
            };
        }  
    }
}