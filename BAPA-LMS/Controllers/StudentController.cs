using BAPA_LMS.DataAccessLayer;
using BAPA_LMS.Models.CourseViewModels;
using BAPA_LMS.Models.DB;
using BAPA_LMS.Models.ModuleViewModels;
using BAPA_LMS.Utils;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
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

            List<Module> moduleList = new List<Module>();
            foreach (var item in db.Modules)
            {
                if (item.CourseId == course.Id)
                {
                    moduleList.Add(item);
                }
            }
            return View(moduleList);
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