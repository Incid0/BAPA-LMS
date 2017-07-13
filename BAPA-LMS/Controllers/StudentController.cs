using BAPA_LMS.DataAccessLayer;
using BAPA_LMS.Models;
using BAPA_LMS.Models.ActivityViewModels;
using BAPA_LMS.Models.CourseViewModels;
using BAPA_LMS.Models.DB;
using BAPA_LMS.Models.ModuleViewModels;
using BAPA_LMS.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;




namespace BAPA_LMS.Controllers
{
    [Authorize]
    public class StudentController : Controller
    {

        private LMSDbContext db = new LMSDbContext();

        public object ModuleDetailViewModel { get; private set; }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult KursInfo()
        {
          
            var currentUser = UserUtils.GetCurrentUser(HttpContext);

            Course course = db.Courses.Find(currentUser.CourseId);
            if(course == null)
            {
                return HttpNotFound();
            }
            CourseDetailViewModel cdvm = course;
         


            return View(cdvm);
        }
        public ActionResult ModulInfo()
        {
            var currentUser = UserUtils.GetCurrentUser(HttpContext);

            Course course = db.Courses.Find(currentUser.CourseId);
            List<ModuleDetailViewModel> moduleList = new List<ModuleDetailViewModel>();
            foreach (var item in db.Modules)
            {
                if(item.CourseId == course.Id.Decode())
                {
                    moduleList.Add(item);
                }
            }  
          
            return View(moduleList);

        }

        public ActionResult AktivitetsInfo(int id)
        {
            var currentUser = UserUtils.GetCurrentUser(HttpContext);
            Course course = db.Courses.Find(currentUser.CourseId);
            List<ActivityDetailViewModel> activityList = new List<ActivityDetailViewModel>();
            foreach (var item in db.Activities)
            {
                if(item.ModuleId == id.Decode())
                {
                    activityList.Add(item);
                }
            }
            return View(activityList);
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