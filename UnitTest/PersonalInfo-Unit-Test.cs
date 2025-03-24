using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AddressValidatorLibrary;
using Basic_information_library.Models;
using System.Threading.Tasks;

namespace UnitTest
{
    [TestClass]
    public class UnitTest2
    {
        [TestMethod]
        public void Constructor_WithValidInputs_CreatesPersonalInfoObject()
        {
            // Arrange
            string fname = "Jake";
            string lname = "Smith"; // Assuming a placeholder surname
            DateTime birthday = new DateTime(1995, 5, 20); // Adjusted birthday for example
            string country = "Philippines";
            string province = "Cebu";
            string city = "Cebu City";
            int houseNumber = 10;
            string street = "Colon Street";
            string barangay = "Barangay Santo Niño";
            int postalCode = 6000;

            // Act
            PersonalInfo info = new PersonalInfo(
                fname, lname, birthday, country, province, city,
                houseNumber, street, barangay, postalCode);

            // Assert
            Assert.AreEqual(fname, info.Fname);
            Assert.AreEqual(lname, info.Lname);
            Assert.AreEqual(birthday, info.Birthday);
            Assert.AreEqual(country, info.Country);
            Assert.AreEqual(province, info.Province);
            Assert.AreEqual(city, info.City);
            Assert.AreEqual(houseNumber, info.HouseNumber);
            Assert.AreEqual(street, info.Street);
            Assert.AreEqual(barangay, info.Barangay);
            Assert.AreEqual(postalCode, info.PostalCode);
            Assert.IsFalse(info.IsAddressVerified);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_WithNullFirstName_ThrowsArgumentNullException()
        {
            // Act - this should throw an exception
            new PersonalInfo(
                null, "Smith", new DateTime(1995, 5, 20), "Philippines", "Cebu", "Cebu City",
                10, "Colon Street", "Barangay Santo Niño", 6000);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_WithNullLastName_ThrowsArgumentNullException()
        {
            // Act - this should throw an exception
            new PersonalInfo(
                "Jake", null, new DateTime(1995, 5, 20), "Philippines", "Cebu", "Cebu City",
                10, "Colon Street", "Barangay Santo Niño", 6000);
        }

        [TestMethod]
        public void CalculateAge_WithBirthdayInPast_ReturnsCorrectAge()
        {
            // Arrange
            int expectedYears = 30; // Example age
            DateTime birthday = DateTime.Today.AddYears(-expectedYears);
            PersonalInfo info = new PersonalInfo(
                "Jake", "Smith", birthday, "Philippines", "Cebu", "Cebu City",
                10, "Colon Street", "Barangay Santo Niño", 6000);

            // Act
            int calculatedAge = info.CalculateAge();

            // Assert
            Assert.AreEqual(expectedYears, calculatedAge);
        }

        [TestMethod]
        public void CalculateAge_WithBirthdayTomorrow_ReturnsPreviousYear()
        {
            // Arrange
            int expectedYears = 30; // Example age
            DateTime birthday = DateTime.Today.AddDays(1).AddYears(-expectedYears - 1);
            birthday = birthday.AddDays(1); // Adjust for tomorrow
            PersonalInfo info = new PersonalInfo(
                "Jake", "Smith", birthday, "Philippines", "Cebu", "Cebu City",
                10, "Colon Street", "Barangay Santo Niño", 6000);

            // Act
            int calculatedAge = info.CalculateAge();

            // Assert
            Assert.AreEqual(expectedYears, calculatedAge,
                "Age should be calculated correctly when birthday hasn't occurred yet this year");
        }

        [TestMethod]
        public async Task ValidateAddress_WithValidAddress_SetsIsAddressVerifiedToTrue()
        {
            // Arrange - Use a known valid address
            PersonalInfo info = new PersonalInfo(
                "Jake", "Smith", new DateTime(1995, 5, 20),
                "Philippines", "Cebu", "Cebu City",
                10, "Colon Street", "Barangay Santo Niño", 6000,
                new AddressValidator());

            // Act
            bool result = await info.ValidateAddress();

            // Assert
            Assert.IsTrue(result);
            Assert.IsTrue(info.IsAddressVerified);
        }
    }
}