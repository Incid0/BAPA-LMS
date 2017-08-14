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
	public class ActivitiesController : BaseController
	{
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
			advm.Files = activity.Files.ToArray();

            return PartialView("_Details", advm);
		}

		// GET: Activities/Create
		[Authorize(Roles = "Admin")]
		public ActionResult Create(int? id)
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
			ActivityEditViewModel aevm = new ActivityEditViewModel();
            Session["moduleid"] = module.Id;
			aevm.Types = db.ActivityTypes.ToList();
			aevm.ModuleStartDate = module.StartDate.ToString("yyyy-MM-dd");
			aevm.ModuleEndDate = module.EndDate.ToString("yyyy-MM-dd");
			return PartialView("_Create", aevm);
		}

		// POST: Activities/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Roles = "Admin")]
		public ActionResult Create(ActivityEditViewModel aevm)
		{
			string returnView = "_Create";
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
						aevm = newActivity; // ActivityEditViewModel
						Session["activityid"] = newActivity.Id;
						TempData["alert"] = "success|Aktiviteten är tillagd!|a" + newActivity.Id.Encode();
						returnView = "_Edit";
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
			return PartialView(returnView, aevm);
		}

		// GET: Activities/Edit/5
		[Authorize(Roles = "Admin")]
		public ActionResult Edit(int? id)
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
			ActivityEditViewModel aevm = activity;
			Session["activityid"] = activity.Id;
            aevm.Types = db.ActivityTypes.ToList();
			aevm.ModuleStartDate = activity.Module.StartDate.ToString("yyyy-MM-dd");
			aevm.ModuleEndDate = activity.Module.EndDate.ToString("yyyy-MM-dd");
			return PartialView("_Edit", aevm);
		}

		// POST: Activities/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Roles = "Admin")]
		public ActionResult Edit(ActivityEditViewModel aevm)
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
				if (updatedActivity != null && TryUpdateModel(updatedActivity, "", new string[] { "Name", "Description", "StartTime", "EndTime" }))
				{
					try
					{
						updatedActivity.TypeId = aevm.Type;
						updatedActivity.StartTime = aevm.StartDate.Date + updatedActivity.StartTime.TimeOfDay;
						updatedActivity.EndTime = aevm.EndDate.Date + updatedActivity.EndTime.TimeOfDay;
						db.SaveChanges();
						aevm = updatedActivity; // ActivityEditViewModel
						TempData["alert"] = "success|Aktiviteten är uppdaterad!|a" + updatedActivity.Id.Encode();
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
			aevm.Types = db.ActivityTypes.ToList();
			return PartialView("_Edit", aevm);
		}

		// GET: Activities/Delete/5
		public ActionResult Delete(int? id)
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
			Session["activityid"] = activity.Id;
			return PartialView("_Delete", (ActivityDeleteViewModel)activity);
		}

		// POST: Activities/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		[Authorize(Roles = "Admin")]
		public ActionResult DeleteConfirmed(ActivityDeleteViewModel advm)
		{
			int? id = (int?)Session["activityid"];
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			try
			{
				Activity activity = db.Activities.Find(id);
				DeleteDocs(activity.Files.ToArray());
				db.Activities.Remove(activity);
				db.SaveChanges();
				TempData["alert"] = "success|Aktiviteten togs bort!";
			}
			catch (RetryLimitExceededException)
			{
				// Log errors here
				TempData["alert"] = "danger|Det gick inte att ta bort aktiviteten!";
			}
			return PartialView("_Delete", advm);
		}

		private string GetColor(Activity activity, ApplicationUser user)
		{
			string result = activity.Type.Color;
			if (activity.Type.Submit)
			{
				if (activity.Files.Where(f => f.MemberId == user.Id).Any())
				{
					result = "#5cb85c"; // Btn-Success
				}
				else if (activity.EndTime < DateTime.Now)
				{
					result = "#c9302c"; // Btn-Danger
				}
			}
			else if (activity.EndTime <= DateTime.Now)
			{
				result = "Gray";
			};
			return result;
		}

		private string GetIcons(Activity activity, ApplicationUser user)
		{
			string result = "";
			if (activity.Files.Where(f => f.CourseName == null).Any())
			{
				result = "file";
			}
			if (activity.Type.Submit && (!activity.Files.Where(f => f.MemberId == user.Id).Any()) && activity.EndTime <= (DateTime.Today + new TimeSpan(5, 0, 0, 0)))
			{
				result += " exclamation-sign";
			}
			return result;
		}

		public JsonResult GetStudentActivities()
		{
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
					color = GetColor(item, user),
					url = "/activities/details/" + item.Id.Encode(),
					className = "modal-link",
					icon = GetIcons(item, user)
				}).ToArray();

			return Json(actArray, JsonRequestBehavior.AllowGet);
		}
	}
}
