using BAPA_LMS.Models.ActivityViewModels;
using BAPA_LMS.Models.DB;
using BAPA_LMS.Utils;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace BAPA_LMS.Controllers
{
    public class FileUploadController : Controller
    {
        private DataAccessLayer.LMSDbContext db = new DataAccessLayer.LMSDbContext();

        public ActionResult ActivityUpload()
        {
            var currentUser = UserUtils.GetCurrentUser(HttpContext);
            Course course = db.Courses.Find(currentUser.CourseId);
            List<ActivityUploadViewModel> activityList = new List<ActivityUploadViewModel>();
            foreach (var item in db.Modules)
            {
                if (course.Id == item.CourseId)
                {
                    foreach (var item2 in db.Activities)
                    {
                        if (item2.ModuleId == item.Id && item2.Type.Id == 3)
                        {
                            activityList.Add(item2);
                        }

                    }

                }
            }
            return View(activityList);
        }
        
        public ActionResult ActivityUploader(int id)
        {
            Activity activity = db.Activities.Find(id);
            ActivityUploadViewModel auvm = activity;
           
   

            return View(auvm);
        }
    
        [HttpPost]
        public ActionResult ActivityUploader(HttpPostedFileBase postedFile, int id)
        {

            Activity activity = db.Activities.Find(id);
            ActivityUploadViewModel auvm = activity;



           
            var currentUser = UserUtils.GetCurrentUser(HttpContext);
            
            if (postedFile != null)
            {
                string path = Server.MapPath("~/Uploads/" + currentUser.Course.Name + "/" +auvm.Name + "/" + currentUser.Email + "/");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                postedFile.SaveAs(path + Path.GetFileName(postedFile.FileName));
                ViewBag.Message = "File uploaded successfully.";
            }

            return View(auvm);
        }
       
    }
}