﻿using BAPA_LMS.Models.DB;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BAPA_LMS.Models.UploadViewModels
{
    public class TeacherUploadViewModel
    {

       

        [Display(Name = "Namn")]
        public string ActivityName { get; set; }
        public string Id { get; set; }
        public string ModuleName { get; set; }
        public string CourseName { get; set; }
        public ICollection<FileDocument> Files { get; set; }


      
    }
}