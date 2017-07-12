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

namespace BAPA_LMS.Controllers
{
    [Authorize]
    public class ModulesController : Controller
    {
        private LMSDbContext db = new LMSDbContext();

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
            Module module = db.Modules.Find(id);
            if (module == null)
            {
                return HttpNotFound();
            }
            ModuleDetailViewModel mdvm = module;
            return View(mdvm);
        }

        // GET: Modules/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Modules/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create(ModuleCreateViewModel mcvm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Module newModule = new Module();
                    // Match up fieldnames and update the model.
                    if(TryUpdateModel(newModule, "", new string[] { "Name", "Description", "StartDate", "EndDate" }))
                    {
                        db.Modules.Add(newModule);
                        db.SaveChanges();
                        TempData["alert"] = "success|Modulen är tillagd!";
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
            return View(mcvm);
        }

        // GET: Modules/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Module module = db.Modules.Find(id);
            if (module == null)
            {
                return HttpNotFound();
            }
            ModuleEditViewModel mevm = module;
            HttpContext.Session["moduleid"] = id;
            return View(mevm);
        }

        // POST: Modules/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit()
        {
            int? id = (int?)HttpContext.Session["moduleid"];
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Module updatedModule = null;
            if (ModelState.IsValid)
            {
                updatedModule = db.Modules.Find(id);
                // Match up fieldnames and update the model.
                if(id != null && TryUpdateModel(updatedModule, "", new string[] { "Name", "Description", "StartDate", "EndDate" }))
                {
                    try
                    {
                        db.SaveChanges();
                        TempData["alert"] = "success|Modulen är uppdaterad!";
                        return RedirectToAction("Index");
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
            return View((ModuleEditViewModel)updatedModule);
        }

        // GET: Modules/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Module module = db.Modules.Find(id);
        //    if (module == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(module);
        //}

        // POST: Modules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Module module = db.Modules.Find(id);
                db.Modules.Remove(module);
                db.SaveChanges();
            }
            catch (RetryLimitExceededException)
            {
                // Log errors here
                TempData["alert"] = "danger|Det gick inte att ta bort modulen";
            }           
            return RedirectToAction("Index");
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
