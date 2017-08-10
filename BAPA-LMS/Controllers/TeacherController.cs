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

        public ActionResult Index(CourseListViewModel clvm)
        {
            var result = db.Courses
                .AsNoTracking()
                .Select(c => new CourseListRow { Id = c.Id, Name = c.Name, Description = c.Description, StartDate = c.StartDate });

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
                result = result.Where(c => c.StartDate >= startDate);
            }
            if (DateTime.TryParse(clvm.EndRange, out DateTime endDate))
            {
                endDate += new TimeSpan(23, 59, 59);
                result = result.Where(c => c.StartDate <= endDate);
            }
            // Order by parameter
            string[] sortdir = (clvm.SortParam ?? "").Split('_');
            if(sortdir.Length == 1)
            {
                switch (sortdir[0])
                {
                    default: result = result.OrderBy(c => c.Name); break;
                    case "StartDate": result = result.OrderBy(c => c.StartDate); break;
                }
            }
            else
            {
                switch (sortdir[0])
                {
                    default: result = result.OrderByDescending(c => c.Name); break;
                    case "StartDate": result = result.OrderByDescending(c => c.StartDate); break;
                }
            }
            // Paginate the result
            clvm.Count = result.Count();
            clvm.Courses = result
                .Skip(() => clvm.Offset)
                .Take(() => CourseListViewModel.PageSize)
                .ToArray();

            if (Request.IsAjaxRequest())
            {
                return PartialView("_TeacherIndex", clvm);
            }
            return View(clvm);

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
            return PartialView("_StudentList", cdvm);
        }

        // POST: /Account/Register
        [HttpPost]
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
            int? courseId = null;
            try
            {
                ApplicationUser delObj = db.Users.SingleOrDefault(u => u.Id == id);
                courseId = delObj.CourseId;
                db.Users.Remove(delObj);
                db.SaveChanges();
            }
            catch (RetryLimitExceededException)
            {
                // Log errors here				
                TempData["alert"] = "danger|Det gick inte att ta bort användaren!";
            }
            return RedirectToAction("CourseEdit", new { id = courseId });
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