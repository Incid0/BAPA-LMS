using BAPA_LMS.Models.DB;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BAPA_LMS.Utils
{
	public sealed class UserUtils
	{
		private UserUtils() { }

		public static ApplicationUser GetCurrentUser(HttpContextBase httpcontext)
		{
			return httpcontext.User.Identity.IsAuthenticated ? httpcontext.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(httpcontext.User.Identity.GetUserId()) : null;
		}
	}
}