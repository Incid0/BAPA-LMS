using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BAPA_LMS.Models;
using BAPA_LMS.Models.CourseViewModels;

namespace BAPA_LMS.Tests
{
	[TestClass]
	public class ModelExtensionsTests
	{
		[TestMethod]
		public void Scramble_Model_Id()
		{
			// arrange
			int encodeValue = 20, decodeValue = 518690578, encodeExpected = 518690578, decodeExpected = 20;

			// Seed our id scrambler
			ModelExtensions.Optimus(1580030173, 59260789, 1163945558);

			// act
			int encodedId = encodeValue.Encode();
			int decodedId = decodeValue.Decode();

			// assert
			Assert.AreEqual(encodeExpected, encodedId, 0, "Id scrambler not working correctly!");
			Assert.AreEqual(decodeExpected, decodedId, 0, "Id scrambler not working correctly!");
		}
	}
}
