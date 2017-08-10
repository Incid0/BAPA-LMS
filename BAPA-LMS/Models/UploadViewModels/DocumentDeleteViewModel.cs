using BAPA_LMS.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BAPA_LMS.Models.UploadViewModels
{
    public class DocumentDeleteViewModel
    {               
            public string Name { get; set; }

            public static implicit operator DocumentDeleteViewModel(FileDocument model)
            {
                return new DocumentDeleteViewModel
                {
                    Name = model.Name
                };
            }        
    }
}