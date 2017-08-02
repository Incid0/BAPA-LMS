using BAPA_LMS.Models.ActivityViewModels;
using BAPA_LMS.Models.DB;

using BAPA_LMS.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;
using BAPA_LMS.DataAccessLayer;

namespace BAPA_LMS.Controllers
{
    public class UploadDocumentController : Controller
    {
        private LMSDbContext db = new LMSDbContext();
        public ActionResult ActivityUpload()
        {
            var currentUser = UserUtils.GetCurrentUser(HttpContext);
            Course course = db.Courses.Find(currentUser.CourseId);
            List<ActivityUploadViewModel> activityList = new List<ActivityUploadViewModel>();
            foreach (var activity in db.Activities)
            {
                if (activity.Type.Id == 3 && activity.Module.CourseId == course.Id)
                {
                    
                    foreach (var file in db.Files)
                    {
                        if (file.Email == currentUser.Email && file.ActivityName == activity.Name)
                        {
                            activity.DocumentIsUploaded = true;
                        }
                    }
                    activityList.Add(activity);
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
                FileDocument file = new FileDocument();
                file.ActivityName = auvm.Name;
                file.Email = currentUser.Email;
                file.CourseName = currentUser.Course.Name;

                file.Name = postedFile.FileName;
                //file.PostedFile = postedFile;


                string path = Server.MapPath("~/Uploads/" + currentUser.Course.Name + "/" + auvm.Name + "/" + currentUser.Email + "/");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                db.Files.Add(file);
                db.SaveChanges();
                postedFile.SaveAs(path + Path.GetFileName(postedFile.FileName));
                ViewBag.Message = "File uploaded successfully.";
            }

            return View(auvm);
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
