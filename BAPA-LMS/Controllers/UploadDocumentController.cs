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
using System.Net;
using System.Data.Entity.Infrastructure;
using System.Data;

namespace BAPA_LMS.Controllers
{
    [Authorize]
    public class UploadDocumentController : BaseController
    {
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
            return PartialView("_ActivityUploader",asvm);
        }

        //Studentupload
        [HttpPost]
        public ActionResult ActivityUploader(HttpPostedFileBase postedFile, int id) 
        {
            Activity activity = db.Activities.Find(id);
            ActivitySubmitViewModel asvm = activity;
            var currentUser = UserUtils.GetCurrentUser(HttpContext);
           
            try
            {
                if (postedFile != null)
                {
                    FileDocument file = new FileDocument();
                    file.Email = currentUser.Email;
                    file.CourseName = currentUser.Course.Name;
                    file.Name = postedFile.FileName;
                    file.ActivityId = id;
                    file.ActivityName = asvm.Name;
                    file.MemberId = currentUser.Id;           
                    file.TimeStamp = DateTime.Now;

                    string path = Server.MapPath("~/Uploads/");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    db.Files.Add(file);
                    db.SaveChanges();
                    file.Id.Encode().ToString();
                    postedFile.SaveAs(path + file.Id.Encode().ToString());
                    TempData["alert"] = "success|Dokumentet är uppladdad!";
                }
                else
                {
                    TempData["alert"] = "danger|Kunde inte lägga till dokument";
                }
            }
            catch (DataException)
            {
                // Log errors here
                TempData["alert"] = "danger|Allvarligt fel!";
            }
            //return PartialView("_ActivityUploader",asvm);
            return RedirectToAction("ActivityUpload");
        }
        
        public ActionResult TeacherUploader(string id)
        {
            TeacherUploadViewModel tuvm = new TeacherUploadViewModel();
			tuvm.Id = id;
			int intId;
            if (int.TryParse(id.Substring(1), out intId))
            {
                switch (id[0])
                {
                    case 'a':
                        Activity activity = db.Activities.Find(intId);
                        tuvm.ActivityName = activity.Name;
                        tuvm.Files = activity.Files;
                        tuvm.CourseName = activity.Module.Course.Name;
                        tuvm.ActivityType = activity.Type.Name;
                        tuvm.Module = activity.Module;
                        break;
                    case 'm':
                        Module module = db.Modules.Find(intId);
                        tuvm.Files = module.Files;
                        tuvm.ModuleName = module.Name;
                        tuvm.CourseName = module.Course.Name;
                        break;
                    case 'c':
                        Course course = db.Courses.Find(intId);
                        tuvm.Files = course.Files;
                        tuvm.CourseName = course.Name;
                        break;
                }
            }
            return PartialView("_TeacherUploader", tuvm);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult TeacherUploader(HttpPostedFileBase postedFile, string id, bool refresh = false)
        {
            TeacherUploadViewModel tuvm = new TeacherUploadViewModel();
			tuvm.Id = id;
			tuvm.Files = new List<FileDocument>();
			var currentUser = UserUtils.GetCurrentUser(HttpContext);
            try
            {
				int intId;
				if (int.TryParse(id.Substring(1), out intId))
                {
                    FileDocument file = new FileDocument();
                    file.MemberId = currentUser.Id;
                    file.TimeStamp = DateTime.Now;
                    switch (id[0])
                    {
                        case 'a':
                            Activity activity = db.Activities.Find(intId);
                            file.ActivityId = activity.Id;
                            tuvm.ActivityName = activity.Name;
                            tuvm.Files = activity.Files;
                            activity.DeadLine = tuvm.DeadLine;
                            break;
                        case 'm':
                            Module module = db.Modules.Find(intId);
                            file.ModuleId = module.Id;
                            tuvm.ModuleName = module.Name;
                            tuvm.Files = module.Files;
                            break;
                        case 'c':
                            Course course = db.Courses.Find(intId);
                            file.CourseId = course.Id;
                            tuvm.CourseName = course.Name;
                            tuvm.Files = course.Files;
                            break;
                        default:
                            break;
                    }
					if (!refresh)
					{
						if (postedFile != null)
						{
							file.Name = postedFile.FileName;

							string path = Server.MapPath("~/Uploads/");
							if (!Directory.Exists(path))
							{
								Directory.CreateDirectory(path);
							}
							db.Files.Add(file);
							db.SaveChanges();
							postedFile.SaveAs(path + file.Id.Encode().ToString());
							TempData["alert"] = "success|Dokumentet är uppladdat!";
						}
						else
						{
							TempData["alert"] = "danger|Kunde inte lägga till dokument";
						}
					}
                }
            }
            catch (Exception)
            {
                TempData["alert"] = "danger|Allvarligt fel!";
            }
            return PartialView("_TeacherUploader", tuvm);
        }

        public ActionResult ListUploadActivities(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id?.Decode());

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
            foreach (var item in db.Files.Where(a => a.Activity.Id == id && a.CourseName != null))
            {
                fileList.Add(item);
            }
          
            var tuple = new Tuple<List<ApplicationUser>, List<FileDocument>>(classList, fileList);
            return View(tuple);
        }

        public FileResult DownloadFile(int id)
        {
            FileDocument fileDocument = db.Files.Find(id);
            string file = "~/Uploads/" + fileDocument.Id.Encode();
            string contentType = ".jpg";

            return File(file, contentType, fileDocument.Name);
        }

        // GET: Document/Delete/5
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
            return PartialView("_Delete", (DocumentDeleteViewModel)file);
        }

        // POST: Document/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed()
        {
            int? id = (int?)Session["fileid"];

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
			FileDocument file = db.Files.Find(id);
			if (file != null && DeleteDocs(new FileDocument[] { file }))
			{
				TempData["alert"] = "success|Dokumentet togs bort!|Upload";
			}
			else
			{
				TempData["alert"] = "danger|Det gick inte att ta bort Dokumentet!";
			}
            return PartialView("_Delete");
        }
    }
}
