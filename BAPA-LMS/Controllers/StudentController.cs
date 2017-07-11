using BAPA_LMS.DataAccessLayer;
using BAPA_LMS.Models.CourseViewModels;
using BAPA_LMS.Models.DB;
using BAPA_LMS.Models.ModuleViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Web.Mvc;



namespace BAPA_LMS.Controllers
{
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
            var userID = User.Identity.GetUserId();
         
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new LMSDbContext()));

            var currentUser = manager.FindById(User.Identity.GetUserId());

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
            var userID = User.Identity.GetUserId();

            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new LMSDbContext()));

            var currentUser = manager.FindById(User.Identity.GetUserId());

            Course course = db.Courses.Find(currentUser.CourseId);

            List <Module> moduleList = new List<Module>();
            foreach (var item in db.Modules)
            {
                if(item.CourseId == course.Id)
                {
                    moduleList.Add(item);
                }
            }

            
          
        
            return View(moduleList);
        }

      
    }
}