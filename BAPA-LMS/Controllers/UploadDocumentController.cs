using BAPA_LMS.Models.ActivityViewModels;
using BAPA_LMS.Models.DB;

using BAPA_LMS.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;
using BAPA_LMS.DataAccessLayer;
using System.Linq;

namespace BAPA_LMS.Controllers
{
    public class UploadDocumentController : Controller
    {
        private LMSDbContext db = new LMSDbContext();
        public ActionResult ActivityUpload()
        {
            var currentUser = UserUtils.GetCurrentUser(HttpContext);
            Course course = db.Courses.Find(currentUser.CourseId);
                     
            List<ActivitySubmitViewModel> activityList = new List<ActivitySubmitViewModel>();
            foreach (var module in course.Modules)
            {
                foreach (var activity in module.Activities)
                {
                    if(activity.Type.Submit) 
                    {
                        activityList.Add(activity);
                    }
                }
            }
            return View(activityList);
        }

        public ActionResult ActivityUploader(int id)
        {
            Activity activity = db.Activities.Find(id);
            ActivitySubmitViewModel asvm = activity;
            return View(asvm);
        }

        [HttpPost]
        public ActionResult ActivityUploader(HttpPostedFileBase postedFile, int id)
        {
            Activity activity = db.Activities.Find(id);
            ActivitySubmitViewModel asvm = activity;
            var currentUser = UserUtils.GetCurrentUser(HttpContext);

            if (postedFile != null)
            {
                FileDocument file = new FileDocument();     
                file.Email = currentUser.Email;
                file.CourseName = currentUser.Course.Name;
                file.Name = postedFile.FileName;
                file.ActivityId = id;
                file.ActivityName = asvm.Name;
                file.MemberId = currentUser.Id;

                string path = Server.MapPath("~/Uploads/" + currentUser.Course.Name + "/" + asvm.Name + "/" + currentUser.Email + "/");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                db.Files.Add(file);
                db.SaveChanges();
                postedFile.SaveAs(path + Path.GetFileName(postedFile.FileName));
                ViewBag.Message = "File uploaded successfully.";
            }

            return View(asvm);
        }
        public ActionResult ListDocuments()
        {
            List<FileDocument> fileList = new List<FileDocument>();
            foreach (var item in db.Files)
            {
                fileList.Add(item);
            }
            return View(fileList);
        }
        public ActionResult DownloadFile(int id)
        {
            FileDocument fileDocument = db.Files.Find(id);
            string file = "~/Uploads/" + fileDocument.CourseName + "/" + fileDocument.ActivityName + "/" + fileDocument.Email + "/" + fileDocument.Name;
            string contentType = ".jpg";

            return File(file, contentType, Path.GetFileName(file));
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
