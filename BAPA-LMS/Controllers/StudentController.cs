using BAPA_LMS;
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
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Contexts;
using System.Threading.Tasks;
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
            var currentUser = UserUtils.GetCurrentUser(HttpContext);

            Course course = db.Courses.Find(currentUser.CourseId);
            CourseIndexViewModel cdvm = course;
            return View(cdvm);
        }

    
        public ActionResult KursInfo()
        {

            var currentUser = UserUtils.GetCurrentUser(HttpContext);

            Course course = db.Courses.Find(currentUser.CourseId);
            if (course == null)
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
            foreach (var item in course.Modules)
            {          
                    moduleList.Add(item);             
            }                       
            return View(moduleList);

        }

        public ActionResult AktivitetsInfo(int id)
        {
            var currentUser = UserUtils.GetCurrentUser(HttpContext);
            Module module = db.Modules.Find(id);
            List<ActivityDetailViewModel> activityList = new List<ActivityDetailViewModel>();
            foreach (var item in module.Activities)
            {   
                    activityList.Add(item);               
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