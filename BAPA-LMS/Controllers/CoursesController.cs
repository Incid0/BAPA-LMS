﻿using System;
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

namespace BAPA_LMS.Controllers
{
	public class CoursesController : Controller
	{
		private LMSDbContext db = new LMSDbContext();

		// GET: Courses
		public ActionResult Index(string filter, string sort = "")
		{
			var result = new List<CourseListViewModel>();
            
			foreach(var course in db.Courses)
			{
				result.Add(course);
			}
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
		public ActionResult Create()
		{
			return View();
		}

		// POST: Courses/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(CourseCreateViewModel ccvm)
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
			return View(ccvm);
		}

		// GET: Courses/Edit/5
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