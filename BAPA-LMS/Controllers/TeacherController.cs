using BAPA_LMS.DataAccessLayer;
using BAPA_LMS.Models;
using BAPA_LMS.Models.CourseViewModels;
using BAPA_LMS.Models.DB;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using BAPA_LMS.Utils;
using BAPA_LMS.Models.ActivityViewModels;
using System.Text.RegularExpressions;
using System;
using BAPA_LMS.Models.UserViewModels;

namespace BAPA_LMS.Controllers
{
    [Authorize(Roles = "Admin")]
    public class TeacherController : BaseController
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

		public ActionResult Index(CourseListViewModel clvm)
		{
			clvm.StartRange = DateTime.Today.ToString("yyyy/MM/dd");
			clvm.EndRange = (DateTime.Today + new TimeSpan(14, 0, 0, 0)).ToString("yyyy/MM/dd");
			clvm.SortParam = "StartDate";

			return View(clvm);
		}

		public ActionResult Filtered(CourseListViewModel clvm)
        {
			IQueryable<Course> result = db.Courses.AsNoTracking();

            // Filter based on words
            string[] filters = (clvm.Filter ?? "").Trim().Split();
            for (int i = 0; i < filters.Length; i++)
            {
                string tmpFilter = filters[i].Trim();
                if (tmpFilter.Length > 2)
                {
                    result = result.Where(c => (
                        c.Name.Contains(tmpFilter) ||
                        c.Description.Contains(tmpFilter)));
                }
            }
            // Filter based on dates
            if (DateTime.TryParse(clvm.StartRange, out DateTime startDate))
            {
                result = result.Where(c => c.Modules.Any(m => m.Activities.Any(a => a.StartTime >= startDate)));
            }
            if (DateTime.TryParse(clvm.EndRange, out DateTime endDate))
            {
                endDate += new TimeSpan(23, 59, 59);
                result = result.Where(c => c.Modules.Any(m => m.Activities.Any(a => a.EndTime <= endDate)));
            }
            // Order by parameter
            string[] sortdir = (clvm.SortParam ?? "").Split('_');
            if(sortdir.Length == 1)
            {
                switch (sortdir[0])
                {
                    case "StartDate": result = result.OrderBy(c => c.StartDate); break;
					case "Description": result = result.OrderBy(c => c.Description); break;
					// Default = Name
					default: result = result.OrderBy(c => c.Name); break;
				}
			}
            else
            {
                switch (sortdir[0])
                {
                    case "StartDate": result = result.OrderByDescending(c => c.StartDate); break;
					case "Description": result = result.OrderByDescending(c => c.Description); break;
					// Default = Name
					default: result = result.OrderByDescending(c => c.Name); break;
				}
			}
            // Paginate the result
            clvm.Count = result.Count();
            CourseListRow[] resultArr = result
				.Select(c => new CourseListRow { Id = c.Id, Name = c.Name, Description = c.Description, StartDate = c.StartDate })
                .Skip(() => clvm.Offset)
                .Take(() => CourseListViewModel.PageSize)
                .ToArray();
			// Encode the result
			for (var i = 0; i < resultArr.Length; i++)
			{
				resultArr[i].Id = resultArr[i].Id.Encode();
			}
			clvm.Courses = resultArr;

			return PartialView("_TeacherIndex", clvm);
        }     

        public ActionResult CourseEdit(int? id)
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
            CourseIndexViewModel civm = course;
            return View(civm);
        }

        public ActionResult StudentList(int? id)
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
            CourseDetailViewModel cdvm = course;
            return PartialView("_StudentList", cdvm);
        }

		// GET: /Account/Register
		public ActionResult Register(int? id)
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
			RegisterViewModel rvm = new RegisterViewModel();
			Session["courseid"] = course.Id;
			return PartialView("_Register", rvm);
		}

		// POST: /Account/Register
		[HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel rvm)
        {
			try
			{
				if (ModelState.IsValid)
				{
					ApplicationUser newUser = new ApplicationUser { UserName = rvm.Email, Email = rvm.Email, CourseId = (int)Session["courseid"], FirstName = rvm.FirstName, LastName = rvm.LastName };
					var result = await UserManager.CreateAsync(newUser, rvm.Password);

					if (result.Succeeded)
					{
						result = UserManager.AddToRole(newUser.Id, "Member");
						//await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
						TempData["alert"] = "success|Eleven är registrerad!";
						ModelState.Clear();
						rvm = new RegisterViewModel();
					}
					else
					{
						TempData["alert"] = "danger|Kunde inte lägga till elev!";
						AddErrors(result);
					}
				}

			}
			catch (Exception)
			{
				ModelState.AddModelError("", "Kan inte spara ändringar. Försök igen och om problemet kvarstår kontakta din systemadministratör.");
				TempData["alert"] = "danger|Allvarligt fel!";
			}
			return PartialView("_Register", rvm);
        }

		// GET: Teacher/StudentEdit/5
		public ActionResult EditStudent(string id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			ApplicationUser user = db.Users.Find(id);
			if (user == null)
			{
				return HttpNotFound();
			}
			EditUserViewModel euvm = user;
			Session["userid"] = user.Id;
			return PartialView("_EditStudent", euvm);
		}

		// POST: Teacher/StudentEdit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult EditStudent(EditUserViewModel euvm)
		{
			string id = (string)Session["userid"];
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			if (ModelState.IsValid)
			{
				ApplicationUser updatedUser = db.Users.Find(id);
				// Match up fieldnames and update the model.
				if (updatedUser != null && TryUpdateModel(updatedUser, "", new string[] { "Email", "FirstName", "LastName" }))
				{
					try
					{
						updatedUser.UserName = euvm.Email;
						db.Entry(updatedUser).State = EntityState.Modified;
						// New password?
						if (euvm.Password != null && euvm.Password.Trim() != "")
						{
							PasswordHasher ph = new PasswordHasher();
							string hashed = ph.HashPassword(euvm.Password.Trim());
							updatedUser.PasswordHash = hashed;
							db.Entry(updatedUser).State = EntityState.Modified;
						}
						db.SaveChanges();
						euvm = updatedUser; // EditUserViewModel
						TempData["alert"] = "success|Eleven är uppdaterad!";
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
					TempData["alert"] = "danger|Kunde inte uppdatera elev";
				}
			}
			return PartialView("_EditStudent", euvm);
		}

		public ActionResult RegisterTeacher()
		{
			return View();
		}

		[HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RegisterTeacher(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email, FirstName = model.FirstName, LastName = model.LastName };

                var result = await UserManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    result = UserManager.AddToRole(user.Id, "Admin");
                    //await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    return RedirectToAction("Index", "Teacher");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

		public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ApplicationUser user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
			DeleteUserViewModel duvm = user;
			Session["userid"] = user.Id;
			return PartialView("_Delete", duvm);
        }

		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(DeleteUserViewModel duvm)
        {
			string id = (string)Session["userid"];
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
            try
            {
                ApplicationUser user = db.Users.Find(id);
				if (user != null)
				{
					DeleteDocs(user.Files.ToArray());
					db.Users.Remove(user);
					db.SaveChanges();
					TempData["alert"] = "success|Elev togs bort!|user";
				} else
				{
					TempData["alert"] = "danger|Eleven gick inte att hitta!";
				}
			}
			catch (RetryLimitExceededException)
            {
                // Log errors here				
                TempData["alert"] = "danger|Det gick inte att ta bort eleven!";
            }
            return PartialView("_Delete", duvm);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
    }
}