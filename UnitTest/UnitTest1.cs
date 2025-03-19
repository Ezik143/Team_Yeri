using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Information;
using AddressValidatorLibrary;
using Moq;// for mocking httpclient 
using System.IO;// for stringwriter para no need nata mag user input balik balik

namespace UnitTest
{
    [TestClass]
    public class PersonalInfoTests
    {
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
        public async Task ValidateAddress_SetsIsAddressVerified_WhenValidationSucceeds()
        {
            // Arrange
            var mockValidator = new Mock<IAddressValidator>();
            mockValidator.Setup(v => v.ValidateAddressAsync(
                "456", "Mango Avenue", "Cebu", "Central Visayas", 6000, "Philippines"))
                .ReturnsAsync(true);

            PersonalInfo person = new PersonalInfo(
                "Jake", "Smith", new DateTime(1995, 8, 15),
                "Philippines", "Central Visayas", "Cebu",
                456, "Mango Avenue", "Barangay Lahug", 6000,
                mockValidator.Object);

            // Act
            bool result = await person.ValidateAddress();

            // Assert
            Assert.IsTrue(result);
            Assert.IsTrue(person.IsAddressVerified);
            mockValidator.Verify(v => v.ValidateAddressAsync(
                "456", "Mango Avenue", "Cebu", "Central Visayas", 6000, "Philippines"),
                Times.Once);
        }

        [TestMethod]
        public async Task ValidateAddress_SetsIsAddressVerified_WhenValidationFails()
        {
            // Arrange
            var mockValidator = new Mock<IAddressValidator>();
            mockValidator.Setup(v => v.ValidateAddressAsync(
                "456", "Mango Avenue", "Cebu", "Central Visayas", 6000, "Philippines"))
                .ReturnsAsync(false);

            PersonalInfo person = new PersonalInfo(
                "Jake", "Smith", new DateTime(1995, 8, 15),
                "Philippines", "Central Visayas", "Cebu",
                456, "Mango Avenue", "Barangay Lahug", 6000,
                mockValidator.Object);

            // Act
            bool result = await person.ValidateAddress();

            // Assert
            Assert.IsFalse(result);
            Assert.IsFalse(person.IsAddressVerified);
            mockValidator.Verify(v => v.ValidateAddressAsync(
                "456", "Mango Avenue", "Cebu", "Central Visayas", 6000, "Philippines"),
                Times.Once);
        }

        [TestMethod]
        public void DisplayFullInfo_WritesToConsole()
        {
            // Arrange
            PersonalInfo person = new PersonalInfo(
                "Jake", "Smith", new DateTime(1995, 8, 15),
                "Philippines", "Central Visayas", "Cebu",
                456, "Mango Avenue", "Barangay Lahug", 6000);

            using (var sw = new StringWriter())
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