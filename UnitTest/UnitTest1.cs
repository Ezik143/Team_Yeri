using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Information;
using AddressValidatorLibrary;

namespace UnitTest
{
    [TestClass]
    public class PersonalInfoTests
    {
        [TestMethod]
        public void IsValidName_ValidNames_ReturnsTrue()
        {
            // Arrange
            string firstName = "john";
            string lastName = "Doe";

            // Act
            bool result = PersonalInfo.IsValidName(firstName, lastName);

            // Assert
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsValidName_EmptyFirstName_ReturnsFalse()
        {
            // Arrange
            string firstName = "";
            string lastName = "Doe";

            // Act
            bool result = PersonalInfo.IsValidName(firstName, lastName);

            // Assert
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsValidName_EmptyLastName_ReturnsFalse()
        {
            // Arrange
            string firstName = "John";
            string lastName = "";

            // Act
            bool result = PersonalInfo.IsValidName(firstName, lastName);

            // Assert
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsValidName_NameWithNumbers_ReturnsFalse()
        {
            // Arrange
            string firstName = "John123";
            string lastName = "Doe";

            // Act
            bool result = PersonalInfo.IsValidName(firstName, lastName);

            // Assert
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsValidDay_ValidDate_ReturnsTrue()
        {
            // Arrange
            int year = 2023;
            int month = 2;
            int day = 28;

            // Act
            bool result = PersonalInfo.IsValidDay(year, month, day);

            // Assert
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsValidDay_InvalidDate_ReturnsFalse()
        {
            // Arrange
            int year = 2023;
            int month = 2;
            int day = 29; // Not a leap year

            // Act
            bool result = PersonalInfo.IsValidDay(year, month, day);

            // Assert
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsFalse(result);
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
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(result);
        }

        [TestMethod]
        public void CalculateAge_ReturnCorrectAge()
        {
            // Arrange
            DateTime birthdate = new DateTime(1990, 5, 15);
            PersonalInfo person = new PersonalInfo("John", "Doe", birthdate, "USA", "California", "Los Angeles", 123, "Main St", "Downtown", 90001);

            // Act
            int age = person.CalculateAge();

            // Assert
            int expectedAge = DateTime.Today.Year - 1990;
            if (DateTime.Today < new DateTime(DateTime.Today.Year, 5, 15))
                expectedAge--;

            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(expectedAge, age);
        }

        [TestMethod]
        public async Task ValidateAddress_SetsIsAddressVerified()
        {
            // This test is simplified to avoid HTTP-related issues
            // Arrange
            PersonalInfo person = new PersonalInfo(
                "John", "Doe", new DateTime(1990, 5, 15),
                "USA", "California", "Los Angeles", 123, "Main St", "Downtown", 90001);

            // Act
            bool initialState = person.IsAddressVerified;
            bool result = await person.ValidateAddress();

            // Assert - we're just checking if the method runs without exceptions
            // and that it sets the IsAddressVerified property
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(result, person.IsAddressVerified);
        }

        [TestMethod]
        public void DisplayFullInfo_WritesToConsole()
        {
            // Arrange
            PersonalInfo person = new PersonalInfo(
                "John", "Doe", new DateTime(1990, 5, 15),
                "USA", "California", "Los Angeles", 123, "Main St", "Downtown", 90001);

            using (var sw = new System.IO.StringWriter())
            {
                Console.SetOut(sw);

                // Act
                person.DisplayFullInfo();

                // Assert
                string output = sw.ToString();
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(output.Contains("John Doe"));
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(output.Contains("123 Main St"));
            }
        }
    }
}