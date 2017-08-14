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
        public int? ActivityId { get; set; }
        public string MemberId { get; set; }
        public int? ModuleId { get; set; }
        public int? CourseId { get; set; }
        public DateTime TimeStamp { get; set; }
        public virtual ApplicationUser Member { get; set; }
        public virtual Course Course { get; set; }
        public virtual Module Module { get; set; }
        public virtual Activity Activity { get; set; }

    }

}