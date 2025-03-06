using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Information;
using System.IO;

namespace UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestValidNames()
        {
            Assert.IsTrue(PersonalInfo.IsValidName("John", "Doe")); // Valid name
            Assert.IsFalse(PersonalInfo.IsValidName("John123", "Doe")); // Invalid: Numbers in name
            Assert.IsFalse(PersonalInfo.IsValidName("John", "Doe_")); // Invalid: Special character in name
            Assert.IsFalse(PersonalInfo.IsValidName("", "Doe")); // Invalid: Empty first name
        }

        [TestMethod]
        public void TestValidDays()
        {
            Assert.IsTrue(PersonalInfo.IsValidDay(2024, 2, 29)); // Leap year (valid)
            Assert.IsFalse(PersonalInfo.IsValidDay(2023, 2, 29)); // Non-leap year (invalid)
            Assert.IsTrue(PersonalInfo.IsValidDay(2023, 12, 31)); // December 31 (valid)
            Assert.IsFalse(PersonalInfo.IsValidDay(2023, 4, 31)); // April only has 30 days (invalid)
        }

        [TestMethod]
        public void TestCalculateAge()
        {
            PersonalInfo person = new PersonalInfo("John", "Doe", new DateTime(2000, 1, 1), "", "", "", 0, "", "", 0);
            int expectedAge = DateTime.Today.Year - 2000;
            if (DateTime.Today < new DateTime(DateTime.Today.Year, 1, 1)) // If birthday hasn't happened yet
            {
                expectedAge--;
            }
            Assert.AreEqual(expectedAge, person.CalculateAge());
        }

        [TestMethod]
        public void TestDisplayFullName()
        {
            PersonalInfo person = new PersonalInfo("Jane", "Doe", DateTime.Now, "", "", "", 0, "", "", 0);
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);
                person.DisplayFullName();
                string expectedOutput = "Your full name is: Jane Doe\r\n";
                Assert.AreEqual(expectedOutput, sw.ToString());
            }
        }

        [TestMethod]
        public void TestDisplayAddress()
        {
            PersonalInfo person = new PersonalInfo("John", "Doe", DateTime.Now, "USA", "California", "Los Angeles", 123, "Main St", "Downtown", 90001);
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);
                person.DisplayAddress();
                string expectedOutput = "Your complete address is: 123 Main St, Downtown, Los Angeles, California 90001, USA\r\n";
                Assert.AreEqual(expectedOutput, sw.ToString());
            }
        }
    }
}
