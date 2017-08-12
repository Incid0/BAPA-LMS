using BAPA_LMS.DataAccessLayer;
using BAPA_LMS.Models;
using BAPA_LMS.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BAPA_LMS.Controllers
{
    public class BaseController : Controller
    {
		protected LMSDbContext db = new LMSDbContext();

		protected bool DeleteDocs(FileDocument[] files)
		{
			bool result = true;
			string path = Server.MapPath("~/Uploads/");
			try
			{
				foreach (FileDocument f in files)
				{
					string fullname = path + f.Id.Encode();
					if (System.IO.File.Exists(fullname))
					{
						System.IO.File.Delete(fullname);
					}
					db.Files.Remove(f);
				}
				db.SaveChanges();
			}
			catch (Exception)
			{
				result = false;
			}
			return result;
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				db.Dispose();
			}
			base.Dispose(disposing);
		}
	}
}