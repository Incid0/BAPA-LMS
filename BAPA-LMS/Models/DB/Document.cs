using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BAPA_LMS.Models.DB
{
    public class Document
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<ApplicationUser> Members { get; set; }
        public virtual ICollection<Activity> Documents { get; set; }
    }
}