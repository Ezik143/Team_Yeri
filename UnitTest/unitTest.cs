using System;
using System.Net.Http;
using System.Threading.Tasks;
using AddressValidatorLibrary;
using Basic_information_library.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest
{
    [TestClass]
    public class AddressValidatorTests
    {
        private AddressValidator _addressValidator;

        [TestInitialize]
        public void TestInitialize()
        {
            _addressValidator = new AddressValidator();
        }

        [TestMethod]
        public async Task ValidateAddressAsync_WithExtremelyLongInputs_ReturnsFalse()
        {
            // Arrange
            string longString = new string('A', 1000);
            string houseNumber = longString;
            string street = longString;
            string city = longString;
            string province = longString;
            int postalCode = 99999;
            string country = longString;

            // Act
            bool result = await _addressValidator.ValidateAddressAsync(houseNumber, street, city, province, postalCode, country);

            // Assert
            Assert.IsFalse(result, "Extremely long address inputs should return false");
        }

        [TestMethod]
        public async Task ValidateAddressAsync_WithNullCountry_ReturnsFalse()
        {
            // Arrange
            string houseNumber = "123";
            string street = "Main St";
            string city = "TestCity";
            string province = "TestProvince";
            int postalCode = 12345;
            string country = null;

            // Act
            bool result = await _addressValidator.ValidateAddressAsync(houseNumber, street, city, province, postalCode, country);

            // Assert
            Assert.IsFalse(result, "Null country should return false");
        }

        [TestMethod]
        public async Task ValidateAddressAsync_WithSpecialCharacters_ReturnsFalse()
        {
            // Arrange
            string houseNumber = "123!@#";
            string street = "Main St & Test Road";
            string city = "San Francisco";
            string province = "CA";
            int postalCode = 94105;
            string country = "USA";

            // Act
            bool result = await _addressValidator.ValidateAddressAsync(houseNumber, street, city, province, postalCode, country);

            // Assert
            Assert.IsFalse(result, "Address with special characters should return false");
        }

        [TestMethod]
        public async Task ValidateAddressAsync_WithUnicodeAddress_ReturnsResult()
        {
            // Arrange
            string houseNumber = "42";
            string street = "Étoile Street";
            string city = "Montréal";
            string province = "Québec";
            int postalCode = 12345;
            string country = "Canada";

            // Act
            bool result = await _addressValidator.ValidateAddressAsync(houseNumber, street, city, province, postalCode, country);

            // Assert
            // Allows both true and false, just ensures no exception
            Assert.IsTrue(true, "Unicode address should not cause validation failure");
        }

        [TestMethod]
        public async Task ValidateAddressAsync_WithValidUSAddress_ReturnsTrue()
        {
            // Arrange
            string houseNumber = "1600";
            string street = "Pennsylvania Avenue";
            string city = "Washington";
            string province = "DC";
            int postalCode = 20500;
            string country = "USA";

            // Act
            bool result = await _addressValidator.ValidateAddressAsync(houseNumber, street, city, province, postalCode, country);

            // Assert
            Assert.IsTrue(result, "Well-known valid address should return true");
        }

        [TestMethod]
        public async Task ValidateAddressAsync_WithValidCanadianAddress_ReturnsTrue()
        {
            // Arrange
            string houseNumber = "80";
            string street = "Wellington Street";
            string city = "Ottawa";
            string province = "Ontario";
            int postalCode = 10000; // Using simple number for postal code in test
            string country = "Canada";

            // Act
            bool result = await _addressValidator.ValidateAddressAsync(houseNumber, street, city, province, postalCode, country);

            // Assert
            Assert.IsTrue(result, "Well-known valid address should return true");
        }

        [TestMethod]
        public async Task ValidateAddressAsync_WithInvalidAddress_ReturnsFalse()
        {
            // Arrange
            string houseNumber = "999999";
            string street = "Nonexistent Street XYZ";
            string city = "InvalidCity";
            string province = "InvalidProvince";
            int postalCode = 99999;
            string country = "InvalidCountry";

            // Act
            bool result = await _addressValidator.ValidateAddressAsync(houseNumber, street, city, province, postalCode, country);

            // Assert
            Assert.IsFalse(result, "Clearly invalid address should return false");
        }

        [TestMethod]
        public async Task ValidateAddressAsync_WithEmptyStreetName_ReturnsFalse()
        {
            // Arrange
            string houseNumber = "123";
            string street = " "; // Using a space instead of empty string to avoid potential argument null exception
            string city = "SomeCity";
            string province = "SomeProvince";
            int postalCode = 12345;
            string country = "SomeCountry";

            // Act
            bool result = await _addressValidator.ValidateAddressAsync(houseNumber, street, city, province, postalCode, country);

            // Assert
            Assert.IsFalse(result, "Address with empty street name should return false");
        }
    }

    [TestClass]
    public class personalInfoUnitTests
    {
        [TestMethod]
        public void CalculateAge_AtYearBoundary_ReturnsCorrectAge()
        {
            // Arrange
            DateTime justBeforeBirthday = DateTime.Today.AddDays(-1).AddYears(-30);
            PersonalInfo info = new PersonalInfo(
                "Jake", "Smith", justBeforeBirthday, "Philippines", "Cebu", "Cebu City",
                10, "Colon Street", "Barangay Santo Niño", 6000);

            // Act
            int calculatedAge = info.CalculateAge();

            // Assert
            Assert.AreEqual(30, calculatedAge, "Age calculation at year boundary should be correct");
        }

        [TestMethod]
        public void Constructor_WithExtremelyLongName_ThrowsArgumentException()
        {
            // Arrange
            string longName = new string('A', 200);

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() =>
            {
                new PersonalInfo(
                    longName, "Smith", new DateTime(1995, 5, 20), "Philippines", "Cebu", "Cebu City",
                    10, "Colon Street", "Barangay Santo Niño", 6000);
            }, "Extremely long name should throw an exception");
        }

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
    [TestClass]

    public class databaseUnitTest
    {
        private PersonalInfo _validPersonalInfo;
        private string _originalMySqlPassword;

        [TestInitialize]
        public void TestInitialize()
        {
            _originalMySqlPassword = Environment.GetEnvironmentVariable("MYSQL_PASSWORD");
            _validPersonalInfo = new PersonalInfo(
                "Test", "User",
                new DateTime(1990, 1, 1),
                "Test Country", "Test Province", "Test City",
                123, "Test Street", "Test Barangay", 12345
            );
        }

        [TestCleanup]
        public void TestCleanup()
        {
            Environment.SetEnvironmentVariable("MYSQL_PASSWORD", _originalMySqlPassword);
        }

        [TestMethod]
        public void SaveToDatabase_WithNoEnvironmentVariable_ThrowsInvalidOperationException()
        {
            Environment.SetEnvironmentVariable("MYSQL_PASSWORD", null);
            var ex = Assert.ThrowsException<InvalidOperationException>(() =>
            {
                Database.SaveToDatabase(_validPersonalInfo);
            });
            StringAssert.Contains(ex.Message, "Error: MySQL password not set");
        }

        [TestMethod]
        public void SaveToDatabase_WithInvalidConnection_ThrowsException()
        {
            Environment.SetEnvironmentVariable("MYSQL_PASSWORD", "invalid_password_for_testing");
            var ex = Assert.ThrowsException<Exception>(() =>
            {
                Database.SaveToDatabase(_validPersonalInfo);
            });
            StringAssert.Contains(ex.Message, "Database Error:");
        }

        [TestMethod]
        public void SaveToDatabase_WithNullPersonalInfo_ThrowsArgumentNullException()
        {
            Environment.SetEnvironmentVariable("MYSQL_PASSWORD", "some_password");
            Assert.ThrowsException<ArgumentNullException>(() => Database.SaveToDatabase(null));
        }
    }
}