using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BAPA_LMS.Models
{
	public static class ModelExtensions
	{
		private static int prime = 1, inverse = 1, xor = 0;

		// See the encoding engine
		public static void Optimus(int prime, int inverse, int xor)
		{
			ModelExtensions.prime = prime;
			ModelExtensions.inverse = inverse;
			ModelExtensions.xor = xor;
		}

		// Extend int to be able to encode and decode id's
		public static int Encode(this int id)
		{
			return ((id * prime) & int.MaxValue) ^ xor;
		}

		public static int Decode(this int id)
		{
			return ((id ^ xor) * inverse) & int.MaxValue;
		}
	}
}