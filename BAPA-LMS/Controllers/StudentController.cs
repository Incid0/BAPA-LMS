using BAPA_LMS.DataAccessLayer;
using BAPA_LMS.Models.CourseViewModels;
using BAPA_LMS.Models.DB;
using BAPA_LMS.Models.ModuleViewModels;
using BAPA_LMS.Models.UploadViewModels;
using BAPA_LMS.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;




namespace BAPA_LMS.Controllers
{
    [Authorize]
    public class StudentController : Controller
    {

        private LMSDbContext db = new LMSDbContext();

        public object ModuleDetailViewModel { get; private set; }

        [Authorize(Roles = "Member")]
        public ActionResult Index()
        {
            var currentUser = UserUtils.GetCurrentUser(HttpContext);

            Course course = db.Courses.Find(currentUser.CourseId);
            CourseIndexViewModel cdvm = course;
            return View(cdvm);
        }

    
        public ActionResult CourseInfo()
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

        public ActionResult Acitvityinfo(int id)
        {
            Module module = db.Modules.Find(id);
            ModuleDetailViewModel mdvm = module;
            return View(mdvm);
        }

        public ActionResult ActivityFileList(int id)
        {
            List<StudentDocumentListViewModel> fileList = new List<StudentDocumentListViewModel>();
            foreach (var file in db.Files.Where(f => f.Activity.Id == id))
            {
                fileList.Add(file);
            }
            return PartialView(fileList);
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