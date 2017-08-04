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
using BAPA_LMS.Models.CourseViewModels;
using System.Data.Entity.Infrastructure;
using BAPA_LMS.Models;

namespace BAPA_LMS.Controllers
{
    [Authorize]
	public class CoursesController : Controller
	{
		private LMSDbContext db = new LMSDbContext();

        // GET: Courses
        [Authorize(Roles = "Admin")]
        public ActionResult Index(string filter, string sort = "")
		{
            var result = db.Courses.Select(c => new CourseListRow { Name = c.Name, Description = c.Description, StartDate = c.StartDate }).ToList();
            
			return View(result);
		}

		// GET: Courses/Details/5
		public ActionResult Details(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Course course = db.Courses.Find(id);
			if (course == null)
			{
				return HttpNotFound();
			}
			CourseDetailViewModel cdvm = course;
			// Add extra data to viewmodel here.
			return View(cdvm);
		}

        // GET: Courses/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
		{
			return View();
		}

		// POST: Courses/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create(CourseEditViewModel cevm)
		{
			try
			{
				if (ModelState.IsValid)
				{
					Course newCourse = new Course();
					// Match up fieldnames and update the model.
					if (TryUpdateModel(newCourse, "", new string[] { "Name", "Description", "StartDate" }))
					{
						db.Courses.Add(newCourse);
						db.SaveChanges();
						TempData["alert"] = "success|Kursen är tillagd!";
					}
					else
					{
						TempData["alert"] = "danger|Kunde inte lägga till kurs!";
					}
				}
			}
			catch (DataException)
			{
				// Log errors here
				ModelState.AddModelError("", "Kan inte spara ändringar. Försök igen och om problemet kvarstår kontakta din systemadministratör.");
				TempData["alert"] = "danger|Allvarligt fel!";
			}
			return View(cevm);
		}

        // GET: Courses/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Course course = db.Courses.Find(id);
			if (course == null)
			{
				return HttpNotFound();
			}
			CourseEditViewModel cevm = course;
			HttpContext.Session["courseid"] = id;
			return View(cevm);
		}

		// POST: Courses/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit()
		{
			int? id = (int?)HttpContext.Session["courseid"];
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Course updatedCourse = null;
			if (ModelState.IsValid)
			{
				updatedCourse = db.Courses.Find(id);
				// Match up fieldnames and update the model.
				if (id != null && TryUpdateModel(updatedCourse, "", new string[] { "Name", "Description", "StartDate" }))
				{
					try
					{
						db.SaveChanges();
						TempData["alert"] = "success|Kursen är uppdaterad!";
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
					TempData["alert"] = "danger|Kunde inte uppdatera kursen!";
				}
			}
			return View((CourseEditViewModel)updatedCourse);
		}

		// GET: Courses/Delete/5
		//public ActionResult Delete(int? id)
		//{
		//    if (id == null)
		//    {
		//        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
		//    }
		//    Course course = db.Courses.Find(id);
		//    if (course == null)
		//    {
		//        return HttpNotFound();
		//    }
		//    return View(course);
		//}

		// POST: Courses/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
		{
			try
			{
				Course course = db.Courses.Find(id);
				db.Courses.Remove(course);
				db.SaveChanges();				
			}
			catch (RetryLimitExceededException)
			{
				// Log errors here				
				TempData["alert"] = "danger|Det gick inte att ta bort kursen!";
			}
			return RedirectToAction("Index");
		}

		[Authorize(Roles = "Admin")]
		public JsonResult GetTree(int id)
		{
			Course course = db.Courses.Find(id);

			var actArray = new {
				id = "c" + course.Id.Encode(),
				text = course.Name,
				icon = "glyphicon glyphicon-home",
				nodes = (course.Modules.OrderBy(m => m.StartDate).Select(m => new {
					id = "m" + m.Id.Encode(),
					text = m.Name,
					icon = "glyphicon glyphicon-book",
					nodes = (m.Activities.OrderBy(a => a.StartTime).Select(a => new
					{
						id = "a" + a.Id.Encode(),
						text = a.Name,
						icon = "glyphicon glyphicon-wrench"
					}))
				})).ToArray()
			};

			return Json(actArray, JsonRequestBehavior.AllowGet);
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
