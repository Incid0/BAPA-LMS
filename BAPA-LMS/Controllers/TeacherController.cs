using BAPA_LMS.DataAccessLayer;
using BAPA_LMS.Models;
using BAPA_LMS.Models.CourseViewModels;
using BAPA_LMS.Models.DB;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;





namespace BAPA_LMS.Controllers
{
	[Authorize(Roles = "Admin")]
	public class TeacherController : Controller
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

		private LMSDbContext db = new LMSDbContext();

		public ActionResult Index()
		{
		 List<CourseListViewModel> courseList = new List<CourseListViewModel>();
			foreach (var item in db.Courses)
			{
				courseList.Add(item);
			}

			return View(courseList);
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
		[AllowAnonymous]
		public ActionResult RegisterTeacher()
		{
			return View();
		}

        public ActionResult StudentList(int? id)
        {
            Course course = db.Courses.Find(id?.Decode());
            if (course == null)
            {
                return HttpNotFound();
            }
            CourseDetailViewModel cdvm = course;
            return PartialView("_StudenList", cdvm);
        }
		//
		// POST: /Account/Register
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Register(RegisterViewModel model, int id)
		{
			if (ModelState.IsValid)
			{
				var user = new ApplicationUser { UserName = model.Email, Email = model.Email, CourseId = id, FirstName = model.FirstName, LastName = model.LastName };
				
				var result = await UserManager.CreateAsync(user, model.Password);
			 
				if (result.Succeeded)
				{
					result = UserManager.AddToRole(user.Id, "Member");
					//await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

					

					return RedirectToAction("Index", "Teacher");
				}
				AddErrors(result);
			}
		
			return View(model);
		}
		[HttpPost]
		[AllowAnonymous]
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
            ApplicationUser delObj = db.Users.SingleOrDefault(u => u.Id == id);
            if (delObj == null)
            {
                return HttpNotFound();
            }
            return View(delObj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]   
        public ActionResult DeleteConfirmed(string id)
        {
            try
            {
                ApplicationUser delObj = db.Users.SingleOrDefault(u => u.Id == id);
                db.Users.Remove(delObj);
                db.SaveChanges();
            }
            catch (RetryLimitExceededException)
            {
                // Log errors here				
                TempData["alert"] = "danger|Det gick inte att ta bort kursen!";
            }
            return RedirectToAction("KursInfo");
        }

        private void AddErrors(IdentityResult result)
		{
			foreach (var error in result.Errors)
			{
				ModelState.AddModelError("", error);
			}
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