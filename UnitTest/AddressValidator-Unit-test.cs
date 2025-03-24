using System;
using System.Threading.Tasks;
using AddressValidatorLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        private AddressValidator _addressValidator;

        [TestInitialize]
        public void TestInitialize()
        {
            _addressValidator = new AddressValidator();
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
}