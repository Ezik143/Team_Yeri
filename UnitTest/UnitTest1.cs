using Microsoft.VisualStudio.TestTools.UnitTesting;
using Information;
using System;

namespace PersonalInfoUnitTest
{
    [TestClass]
    public class PersonalInfoTests
    {
        [TestMethod]
        public void TestFullName()
        {
            // Arrange
            var person = new PersonalInfo("Jake", "Smith", new DateTime(1995, 5, 20), "Philippines", "Cebu", "Cebu City", 123, "Mango Ave", "Barangay Luz", 6000);

            // Assert
            Assert.AreEqual("Jake", person.Fname);
            Assert.AreEqual("Smith", person.Lname);
        }

        [TestMethod]
        public void TestAgeCalculation()
        {
            // Arrange
            var person = new PersonalInfo("Jake", "Smith", new DateTime(1995, 5, 20), "Philippines", "Cebu", "Cebu City", 123, "Mango Ave", "Barangay Luz", 6000);

            // Act
            int age = person.CalculateAge();

            // Assert
            int expectedAge = DateTime.Today.Year - 1995;
            if (DateTime.Today < new DateTime(DateTime.Today.Year, 5, 20))
            {
                expectedAge--;
            }
            Assert.AreEqual(expectedAge, age);
        }

        [TestMethod]
        public void TestValidName()
        {
            // Assert
            Assert.IsTrue(PersonalInfo.IsValidName("Jake", "Smith"));
            Assert.IsFalse(PersonalInfo.IsValidName("", "Smith"));
            Assert.IsFalse(PersonalInfo.IsValidName("Jake", ""));
            Assert.IsFalse(PersonalInfo.IsValidName("J@ke", "Smith"));
        }

        [TestMethod]
        public void TestValidDay()
        {
            // Assert
            Assert.IsTrue(PersonalInfo.IsValidDay(2024, 2, 29)); // Leap year
            Assert.IsFalse(PersonalInfo.IsValidDay(2023, 2, 29)); // Non-leap year
            Assert.IsTrue(PersonalInfo.IsValidDay(2023, 4, 30));
            Assert.IsFalse(PersonalInfo.IsValidDay(2023, 4, 31));
            Assert.IsTrue(PersonalInfo.IsValidDay(2023, 1, 31));
        }
    }
}
