using BAPA_LMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BAPA_LMS
{
	public class LMSConfig
	{
		public static void Init()
		{
			// Seed our id scrambler
			ModelExtensions.Optimus(1580030173, 59260789, 1163945558);
		}
	}
}