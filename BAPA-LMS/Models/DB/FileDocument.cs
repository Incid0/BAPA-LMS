using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BAPA_LMS.Models.DB
{
    public class FileDocument
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ActivityName { get; set; }
        public string CourseName { get; set; }
        public string Email { get; set; }
       

        public virtual ApplicationUser Member { get; set; }
       
    }
}