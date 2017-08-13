using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BAPA_LMS.DataAccessLayer;
using BAPA_LMS.Models.DB;
using BAPA_LMS.Models.ModuleViewModels;
using System.Data.Entity.Infrastructure;
using BAPA_LMS.Models;

namespace BAPA_LMS.Controllers
{
    [Authorize]
    public class ModulesController : BaseController
    {
        // GET: Modules
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            var modules = db.Modules.Include(m => m.Course);
            return View(modules.ToList());
        }

        // GET: Modules/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Module module = db.Modules.Find(id?.Decode());
            if (module == null)
            {
                return HttpNotFound();
            }
            ModuleDetailViewModel mdvm = module;
            return View(mdvm);
        }

        // GET: Modules/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create(int? id)
        {
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Course course = db.Courses.Find(id?.Decode());
			if (course == null)
			{
				return HttpNotFound();
			}
			ModuleEditViewModel mevm = new ModuleEditViewModel();
			Session["courseid"] = course.Id;
			mevm.CourseStartDate = course.StartDate.ToString("yyyy-MM-dd");
			return PartialView("_Create", mevm);
        }

        // POST: Modules/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create(ModuleEditViewModel mevm)
        {
			string returnView = "_Create";
			try
			{
                if (ModelState.IsValid)
                {
                    Module newModule = new Module();
                    // Match up fieldnames and update the model.
                    if(TryUpdateModel(newModule, "", new string[] { "Name", "Description", "StartDate", "EndDate" }))
                    {
						newModule.CourseId = (int)Session["courseid"];
						db.Modules.Add(newModule);
                        db.SaveChanges();
						mevm = newModule; // ModuleEditViewModel
						Session["moduleid"] = newModule.Id;
						TempData["alert"] = "success|Modulen är tillagd!|m" + newModule.Id.Encode();
						returnView = "_Edit";
					}
					else
                    {
                        TempData["alert"] = "danger|Kunde inte lägga till modul!";
                    }
                    
                }
            }
            catch (DataException)
            {
                // Log errors here
                ModelState.AddModelError("", "Kan inte spara ändringar. Försök igen och om problemet kvarstår kontakta din systemadministratör.");
                TempData["alert"] = "danger|Allvarligt fel!";
            }            
            return PartialView(returnView, mevm);
        }

        // GET: Modules/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Module module = db.Modules.Find(id?.Decode());
            if (module == null)
            {
                return HttpNotFound();
            }
            ModuleEditViewModel mevm = module;
            Session["moduleid"] = module.Id;
			mevm.CourseStartDate = module.Course.StartDate.ToString("yyyy-MM-dd");
            return PartialView("_Edit", mevm);
        }

        // POST: Modules/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(ModuleEditViewModel mevm)
        {
            int? id = (int?)Session["moduleid"];
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (ModelState.IsValid)
            {
				Module updatedModule = db.Modules.Find(id);
                // Match up fieldnames and update the model.
                if(updatedModule != null && TryUpdateModel(updatedModule, "", new string[] { "Name", "Description", "StartDate", "EndDate" }))
                {
                    try
                    {
                        db.SaveChanges();
						mevm = updatedModule; // ModuleEditViewModel
						TempData["alert"] = "success|Modulen är uppdaterad!|m" + updatedModule.Id.Encode();
                    }
                    catch (RetryLimitExceededException)
                    {
                        // Log errors here
                        ModelState.AddModelError("", "Kan inte spara ändringar. Försök igen och om problemet kvarstår kontakta din systemadministratör.");
                        TempData["alert"] = "danger|Allvarligt fel!";
                    }
                }
                else
                {
                    TempData["alert"] = "danger|Kunde inte uppdatera modulen";
                }
            }
            return PartialView("_Edit", mevm);
        }

		// GET: Modules/Delete/5
		public ActionResult Delete(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Module module = db.Modules.Find(id?.Decode());
			if (module == null)
			{
				return HttpNotFound();
			}
			Session["activityid"] = module.Id;
			return PartialView("_Delete", (ModuleDeleteViewModel)module);
		}

		// POST: Modules/Delete/5
		[HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(ModuleDeleteViewModel mdvm)
        {
			int? id = (int?)Session["moduleid"];
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			try
			{
                Module module = db.Modules.Find(id);
				List<FileDocument> docs = new List<FileDocument>();
				docs.AddRange(module.Files);
				foreach (var activity in module.Activities)
				{
					docs.AddRange(activity.Files);
				}
				DeleteDocs(docs.ToArray());
				db.Modules.Remove(module);
                db.SaveChanges();
				TempData["alert"] = "success|Modulen togs bort!";
			}
			catch (RetryLimitExceededException)
            {
                // Log errors here
                TempData["alert"] = "danger|Det gick inte att ta bort modulen";
            }
			return PartialView("_Delete", mdvm);
        }
    }
}
