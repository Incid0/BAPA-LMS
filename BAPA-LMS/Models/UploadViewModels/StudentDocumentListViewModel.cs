using BAPA_LMS.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BAPA_LMS.Models.UploadViewModels
{
    public class StudentDocumentListViewModel
    {
        public string Name { get; set; }
        public int ActivityId { get; set; }

        public static implicit operator StudentDocumentListViewModel(FileDocument model)
        {
            return new StudentDocumentListViewModel
            {
                Name = model.Name,
                ActivityId = model.Activity.Id

            };
        }
    }
}