using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BAPA_LMS.DataAccessLayer;
using BAPA_LMS.Models;
using BAPA_LMS.Models.DB;
using BAPA_LMS.Models.ActivityViewModels;
using BAPA_LMS.Utils;
using System.Data.Entity.Infrastructure;

namespace BAPA_LMS.Controllers
{
	[Authorize]
	public class ActivitiesController : Controller
	{
		private LMSDbContext db = new LMSDbContext();

		// GET: Activities
		[Authorize(Roles = "Admin")]
		public ActionResult Index()
		{
			var activities = db.Activities.Include(a => a.Module);
			return View(activities.ToList());
		}

		// GET: Activities/Details/5
		public ActionResult Details(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Activity activity = db.Activities.Find(id?.Decode());
			if (activity == null)
			{
				return HttpNotFound();
			}
			ActivityDetailViewModel advm = activity;
			return PartialView("_Details", advm);
		}

		// GET: Activities/Create
		[Authorize(Roles = "Admin")]
		public ActionResult Create(int id)
		{
             var viewModel = new ActivityEditViewModel
            {
                Types = db.ActivityTypes.ToList()
            };
            Session["moduleid"] = id;
            return View(viewModel);
		}

		// POST: Activities/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Roles = "Admin")]
		public ActionResult Create(ActivityEditViewModel aevm)
		{
			try
			{
				if (ModelState.IsValid)
				{
					Activity newActivity = new Activity();
					if (TryUpdateModel(newActivity, "", new string[] { "Name", "Description", "StartTime", "EndTime"  }))
					{
                        newActivity.ModuleId = (int)Session["moduleid"];
                        newActivity.TypeId = aevm.Type;
                        newActivity.StartTime = aevm.StartDate.Date + newActivity.StartTime.TimeOfDay;
                        newActivity.EndTime = aevm.EndDate.Date + newActivity.EndTime.TimeOfDay;
                        db.Activities.Add(newActivity);
						db.SaveChanges();
						TempData["alert"] = "success|Aktiviteten är tillagd!";
					}
					else
					{
						TempData["alert"] = "danger|Kunde inte lägga till aktivitet";
					}
				}
			}
			catch (DataException)
			{
				ModelState.AddModelError("", "Kan inte spara ändringar. Försök igen och om problemet kvarstår kontakta din systemadministratör.");
				TempData["alert"] = "danger|Allvarligt fel!";
			}
            aevm.Types = db.ActivityTypes.ToList();
			return View(aevm);
		}

		// GET: Activities/Edit/5
		[Authorize(Roles = "Admin")]
		public ActionResult Edit(int? id)
		{            
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			Activity activity = db.Activities.Find(id);

			if (activity == null)
			{
				return HttpNotFound();
			}
			ActivityEditViewModel aevm = activity;
			HttpContext.Session["activityid"] = id;
            aevm.Types = db.ActivityTypes.ToList();
			return View(aevm);
		}

		// POST: Activities/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Roles = "Admin")]
		public ActionResult Edit()
		{
			int? id = (int?)HttpContext.Session["activityid"];
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Activity updatedActivity = null;
			if (ModelState.IsValid)
			{
				updatedActivity = db.Activities.Find(id);
				if (id != null && TryUpdateModel(updatedActivity, "", new string[] { "Name", "Description", "StartTime", "EndTime", "Type" }))
				{
					try
					{
						db.SaveChanges();
						TempData["alert"] = "success|Aktiviteten är uppdaterad!";
						return RedirectToAction("Index");
					}
					catch (RetryLimitExceededException)
					{
						ModelState.AddModelError("", "Kan inte spara ändringar. Försök igen och om problemet kvarstår kontakta din systemadministratör.");
						TempData["alert"] = "danger|Allvarligt fel!";
					}
				}
				else
				{
					TempData["alert"] = "danger|Kunde inte uppdatera aktiviteten!";
				}
			}
			return View((ActivityEditViewModel)updatedActivity);
		}

		// GET: Activities/Delete/5
		//public ActionResult Delete(int? id)
		//{
		//    if (id == null)
		//    {
		//        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
		//    }
		//    Activity activity = db.Activities.Find(id);
		//    if (activity == null)
		//    {
		//        return HttpNotFound();
		//    }
		//    return View(activity);
		//}

		// POST: Activities/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		[Authorize(Roles = "Admin")]
		public ActionResult DeleteConfirmed(int id)
		{
			try
			{
				Activity activity = db.Activities.Find(id);
				db.Activities.Remove(activity);
				db.SaveChanges();
			}
			catch (RetryLimitExceededException)
			{
				// LOg errors here
				TempData["alert"] = "danger|Det gick inte att ta bort aktiviteten!";
			}
			return RedirectToAction("Index");
		}

		public JsonResult GetStudentActivities()
		{
			// Hardcoded demodata
			Random rng = new Random();
			string[] activityIcons = new string[] { "", "file", "file" };
			var user = UserUtils.GetCurrentUser(HttpContext);
			var actArray = db.Activities
				.Where(a => a.Module.CourseId == user.CourseId)
				.ToList()
				.Select(item => new
				{
					id = item.Id.Encode(),
					title = item.Name,
					start = item.StartTime,
					end = item.EndTime,
					color = (item.EndTime >= DateTime.Now) || item.Type.Submit ? item.Type.Color : "Gray",
					url = "/activities/details/" + item.Id.Encode(),
					className = "modal-link",
					icon = activityIcons[rng.Next(3)] + (item.Type.Submit ? " exclamation-sign" : "")
				}).ToArray();

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
