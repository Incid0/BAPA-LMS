﻿using BAPA_LMS.DataAccessLayer;
using BAPA_LMS.Models.CourseViewModels;
using BAPA_LMS.Models.DB;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;



namespace BAPA_LMS.Controllers
{
    public class StudentController : Controller
    {

        private LMSDbContext db = new LMSDbContext();
        
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult KursInfo()
        {
            var userID = User.Identity.GetUserId();
         
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new LMSDbContext()));

            var currentUser = manager.FindById(User.Identity.GetUserId());


            Course course = db.Courses.Find(currentUser.CourseId);
            if(course == null)
            {
                return HttpNotFound();
            }
            CourseDetailViewModel cdvm = course;

            return View(cdvm);
        }

      
    }
}