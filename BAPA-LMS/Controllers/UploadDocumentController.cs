using BAPA_LMS.DataAccessLayer;
using BAPA_LMS.Models.ActivityViewModels;
using BAPA_LMS.Models.DB;
using BAPA_LMS.Models;
using BAPA_LMS.Models.UploadViewModels;
using BAPA_LMS.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BAPA_LMS.DataAccessLayer;
using System.Linq;
using System.Net;
using System.Data.Entity.Infrastructure;

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
                    if (activity.Type.Submit)
                    {
                        activityList.Add(activity);
                    }
                }
            }
            List<FileDocument> fileList = new List<FileDocument>();
            foreach (var file in db.Files.Where(u => u.Member.Id == currentUser.Id))
            {
                fileList.Add(file);
            }
            var tuple = new Tuple<List<ActivitySubmitViewModel>, ApplicationUser, List<FileDocument>>(activityList, currentUser, fileList);
            return View(tuple);
        }

        public ActionResult ActivityUploader(int id)
        {
            Activity activity = db.Activities.Find(id);
            ActivitySubmitViewModel asvm = activity;
            return View(asvm);
        }

        [HttpPost]
        public ActionResult ActivityUploader(HttpPostedFileBase postedFile, int id) //Studentupload
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

        public ActionResult TeacherUploader(string id)
        {
            TeacherUploadViewModel tuvm = new TeacherUploadViewModel();
            int intId;
            if (int.TryParse(id.Substring(1), out intId))
            {
                switch (id[0])
                {
                    case 'a':
                        Activity activity = db.Activities.Find(intId);
                        tuvm.ActivityName = activity.Name;
                        tuvm.Files = activity.Files;
                        tuvm.toxicId = id;
                        tuvm.CourseName = activity.Module.Course.Name;
                        break;
                    case 'm':
                        Module module = db.Modules.Find(intId);
                        tuvm.Files = module.Files;
                        tuvm.ModuleName = module.Name;
                        tuvm.toxicId = id;
                        tuvm.CourseName = module.Course.Name;
                        break;
                    case 'c':
                        Course course = db.Courses.Find(intId);
                        tuvm.Files = course.Files;
                        tuvm.CourseName = course.Name;
                        tuvm.toxicId = id;
                        break;
                }
            }
            return View(tuvm);
        }

        [HttpPost]
        public ActionResult TeacherUploader(HttpPostedFileBase postedFile, string id)
        {
            TeacherUploadViewModel tuvm = new TeacherUploadViewModel();
            var currentUser = UserUtils.GetCurrentUser(HttpContext);
            int intId;
            if (int.TryParse(id.Substring(1), out intId))
            {

                if (postedFile != null)
                {
                    FileDocument file = new FileDocument();
                    file.MemberId = currentUser.Id;
                    switch (id[0])
                    {
                        case 'a':
                            Activity activity = db.Activities.Find(intId);
                            file.ActivityId = activity.Id;
                            tuvm.ActivityName = activity.Name;
                            tuvm.Files = activity.Files;
                            tuvm.toxicId = id;
                            break;
                        case 'm':
                            Module module = db.Modules.Find(intId);
                            file.ModuleId = module.Id;
                            tuvm.ModuleName = module.Name;
                            tuvm.Files = module.Files;
                            tuvm.toxicId = id;
                            break;
                        case 'c':
                            Course course = db.Courses.Find(intId);
                            file.CourseId = course.Id;
                            tuvm.CourseName = course.Name;
                            tuvm.Files = course.Files;
                            tuvm.toxicId = id;
                            break;
                        default:
                            break;
                    }
                    file.Name = postedFile.FileName;


                    string path = Server.MapPath("~/Uploads/");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    db.Files.Add(file);
                    db.SaveChanges();
                    postedFile.SaveAs(path + Path.GetFileName(postedFile.FileName));
                    ViewBag.Message = "Uppladdningen lyckades.";
                }
            }
            return View(tuvm);
        }

        public ActionResult ListUploadActivities(int id)
        {

            Course course = db.Courses.Find(id);

            List<ActivitySubmitViewModel> activityList = new List<ActivitySubmitViewModel>();
            foreach (var module in course.Modules)
            {
                foreach (var activity in module.Activities)
                {
                    if (activity.Type.Submit)
                    {
                        activityList.Add(activity);

                    }
                }
            }
            return View(activityList);
        }

        public ActionResult ListDocuments(int id)
        {

            Activity activity = db.Activities.Find(id);
            List<ApplicationUser> classList = new List<ApplicationUser>();
            foreach (var item in db.Users.Where(u => u.Course.Id == activity.Module.Course.Id))
            {

                classList.Add(item);
            }

            List<FileDocument> fileList = new List<FileDocument>();
            foreach (var item in db.Files.Where(a => a.Activity.Id == id))
            {
                fileList.Add(item);
            }
            var tuple = new Tuple<List<ApplicationUser>, List<FileDocument>>(classList, fileList);
            return View(tuple);
        }

        public ActionResult DownloadFile(int id)
        {
            FileDocument fileDocument = db.Files.Find(id);
            string file = "~/Uploads/" + fileDocument.CourseName + "/" + fileDocument.ActivityName + "/" + fileDocument.Email + "/" + fileDocument.Name;
            string contentType = ".jpg";

            return File(file, contentType, Path.GetFileName(file));
        }

        // GET: Activities/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FileDocument file = db.Files.Find(id?.Decode());
            if (file == null)
            {
                return HttpNotFound();
            }
            Session["fileid"] = file.Id;
            return PartialView();
        }

        // POST: Activities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed()
        {
            int? id = (int?)Session["fileid"];
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            try
            {
                FileDocument file = db.Files.Find(id);
                db.Files.Remove(file);
                db.SaveChanges();
                TempData["alert"] = "success|Dokumentet togs bort!";
            }
            catch (RetryLimitExceededException)
            {
                // Log errors here
                TempData["alert"] = "danger|Det gick inte att ta bort Dokumentet!";
            }
            return PartialView();
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
