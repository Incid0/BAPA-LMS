using BAPA_LMS.DataAccessLayer;
using BAPA_LMS.Models;
using BAPA_LMS.Models.ActivityViewModels;
using BAPA_LMS.Models.CourseViewModels;
using BAPA_LMS.Models.DB;
using BAPA_LMS.Models.ModuleViewModels;
using BAPA_LMS.Utils;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Web.Mvc;




namespace BAPA_LMS.Controllers
{
	[Authorize(Roles = "Admin")]
	public class TeacherController : Controller
    {

        private LMSDbContext db = new LMSDbContext();

        public object ModuleDetailViewModel { get; private set; }

        public ActionResult Index()
        {
            var currentUser = UserUtils.GetCurrentUser(HttpContext);
         
            Course course = db.Courses.Find(currentUser.CourseId);
            CourseIndexViewModel cdvm = course;
            return View(cdvm);
        }

        public ActionResult CourseEdit(int? id)
        {
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Course course = db.Courses.Find(id);
			if (course == null)
			{
				return HttpNotFound();
			}
			CourseDetailViewModel cdvm = course;
			return View(cdvm);
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