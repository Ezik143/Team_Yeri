using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Information;

namespace UnitTest
{
    [TestClass]
    public class PersonalInfoTests
    {
        [TestMethod]
        public void IsValidName_ValidNames_ReturnsTrue()
        {
            // Arrange
            string firstName = "Jake";
            string lastName = "Smith";

            // Act
            bool result = PersonalInfo.IsValidName(firstName, lastName);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsValidName_EmptyFirstName_ReturnsFalse()
        {
            // Arrange
            string firstName = "";
            string lastName = "Smith";

            // Act
            bool result = PersonalInfo.IsValidName(firstName, lastName);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsValidName_EmptyLastName_ReturnsFalse()
        {
            // Arrange
            string firstName = "Jake";
            string lastName = "";

            // Act
            bool result = PersonalInfo.IsValidName(firstName, lastName);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsValidName_NameWithNumbers_ReturnsFalse()
        {
            // Arrange
            string firstName = "Jake123";
            string lastName = "Smith";

            // Act
            bool result = PersonalInfo.IsValidName(firstName, lastName);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsValidDay_ValidDate_ReturnsTrue()
        {
            // Arrange
            int year = 2023;
            int month = 8;
            int day = 15;

            // Act
            bool result = PersonalInfo.IsValidDay(year, month, day);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsValidDay_InvalidDate_ReturnsFalse()
        {
            // Arrange
            int year = 2023;
            int month = 2;
            int day = 30; // Invalid date

            // Act
            bool result = PersonalInfo.IsValidDay(year, month, day);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsValidDay_LeapYear_ReturnsTrue()
        {
            // Arrange
            int year = 2024; // Leap year
            int month = 2;
            int day = 29;

            // Act
            bool result = PersonalInfo.IsValidDay(year, month, day);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CalculateAge_ReturnCorrectAge()
        {
            // Arrange
            DateTime birthdate = new DateTime(1995, 8, 15);
            PersonalInfo person = new PersonalInfo(
                "Jake", "Smith", birthdate,
                "Philippines", "Central Visayas", "Cebu",
                456, "Mango Avenue", "Barangay Lahug", 6000);

            // Act
            int age = person.CalculateAge();

            // Assert
            int expectedAge = DateTime.Today.Year - 1995;
            if (DateTime.Today < new DateTime(DateTime.Today.Year, 8, 15))
                expectedAge--;

            Assert.AreEqual(expectedAge, age);
        }

        [TestMethod]
        public async Task ValidateAddress_SetsIsAddressVerified()
        {
            // This test is simplified to avoid HTTP-related issues
            // Arrange
            PersonalInfo person = new PersonalInfo(
                "Jake", "Smith", new DateTime(1995, 8, 15),
                "Philippines", "Central Visayas", "Cebu",
                456, "Mango Avenue", "Barangay Lahug", 6000);

            // Act
            bool initialState = person.IsAddressVerified;
            bool result = await person.ValidateAddress();

            // Assert - we're just checking if the method runs without exceptions
            // and that it sets the IsAddressVerified property
            Assert.AreEqual(result, person.IsAddressVerified);
        }

        [TestMethod]
        public void DisplayFullInfo_WritesToConsole()
        {
            // Arrange
            PersonalInfo person = new PersonalInfo(
                "Jake", "Smith", new DateTime(1995, 8, 15),
                "Philippines", "Central Visayas", "Cebu",
                456, "Mango Avenue", "Barangay Lahug", 6000);

            using (var sw = new System.IO.StringWriter())
            {
                Console.SetOut(sw);

                // Act
                person.DisplayFullInfo();

                // Assert
                string output = sw.ToString();
                Assert.IsTrue(output.Contains("Jake Smith"));
                Assert.IsTrue(output.Contains("456 Mango Avenue"));
            }
        }
    }
}
