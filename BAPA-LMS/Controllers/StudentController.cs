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


namespace BAPA_LMS.Controllers
{
    public class StudentController : Controller
    {

        private LMSDbContext _context = new LMSDbContext();
        public ActionResult Index()
        {
            return View();
        }

        //public ActionResult KursInfo()
        //{
        //    var student = _context.Courses.Where(s => s.)
        //}
    }
}