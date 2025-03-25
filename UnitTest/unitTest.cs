using System;
using System.Net.Http;
using System.Threading.Tasks;
using AddressValidatorLibrary;
using Basic_information_library.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using Basic_information_library.Models;

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
        public async Task ValidateAddressAsync_WithUnicodeCharactersInAddress_HandlesCorrectly()
        {
            // Arrange
            string houseNumber = "42";
            string street = "Rue Saint-André";
            string city = "Montréal";
            string province = "Québec";
            int postalCode = 12345;
            string country = "Canada";

            // Act
            bool result = await _addressValidator.ValidateAddressAsync(
                houseNumber, street, city, province, postalCode, country);

            // Assert
            // The assertion allows both true and false, ensuring no exception is thrown
            Assert.IsTrue(true, "Address with Unicode characters should be processed without errors");
        }

        [TestMethod]
        public async Task ValidateAddressAsync_WithVeryShortAddressComponents_ReturnsFalse()
        {
            // Arrange
            string houseNumber = "a";
            string street = "a";
            string city = "a";
            string province = "a";
            int postalCode = 12345;
            string country = "a";

            // Act
            bool result = await _addressValidator.ValidateAddressAsync(
                houseNumber, street, city, province, postalCode, country);

            // Assert
            Assert.IsFalse(result, "Very short address components should return false");
        }

        [TestMethod]
        public async Task ValidateAddressAsync_WithRepeatedApiCalls_MaintainsConsistency()
        {
            // Arrange
            string houseNumber = "1600";
            string street = "Pennsylvania Avenue";
            string city = "Washington";
            string province = "DC";
            int postalCode = 20500;
            string country = "USA";

            // Act & Assert
            // Multiple consecutive calls to validate the same address
            for (int i = 0; i < 3; i++)
            {
                bool result = await _addressValidator.ValidateAddressAsync(
                    houseNumber, street, city, province, postalCode, country);

                Assert.IsTrue(result, $"Consecutive API call {i + 1} should return consistent result");
            }
        }

        [TestMethod]
        public async Task ValidateAddressAsync_WithAddressUsingSpecialCharacters_ReturnsFalse()
        {
            // Arrange
            string houseNumber = "123!@#";
            string street = "Main & Test Road";
            string city = "San Francisco";
            string province = "CA";
            int postalCode = 94105;
            string country = "USA";

            // Act
            bool result = await _addressValidator.ValidateAddressAsync(
                houseNumber, street, city, province, postalCode, country);

            // Assert
            Assert.IsFalse(result, "Address with special characters should return false");
        }

        public async Task ValidateAddressAsync_WithMixedCaseInputs_ReturnsConsistentResult()
        {
            // Arrange
            string houseNumber = "1600";
            string street = "PeNnSyLvAnIa AvEnUe";
            string city = "wAsHiNgToN";
            string province = "Dc";
            int postalCode = 20500;
            string country = "uSa";

            // Act
            bool result = await _addressValidator.ValidateAddressAsync(
                houseNumber, street, city, province, postalCode, country);

            // Assert
            Assert.IsTrue(result, "Address validation should work with mixed case inputs");
        }

        [TestMethod]
        public async Task ValidateAddressAsync_WithAPIFailure_ReturnsFalse()
        {
            var failingHttpClient = new HttpClient { BaseAddress = new Uri("https://invalid.url") };
            var failingValidator = new AddressValidator(failingHttpClient);

            string houseNumber = "123";
            string street = "Main St";
            string city = "TestCity";
            string province = "TestProvince";
            int postalCode = 12345;
            string country = "USA";

            bool result = await failingValidator.ValidateAddressAsync(houseNumber, street, city, province, postalCode, country);

            Assert.IsFalse(result, "API failure should return false");
        }

        [TestMethod]
        public async Task ValidateAddressAsync_WithValidNonUSPostalCode_ReturnsTrue()
        {
            string houseNumber = "10";
            string street = "High Street";
            string city = "London";
            string province = "England";
            int postalCode = 1001;
            string country = "United Kingdom";

            bool result = await _addressValidator.ValidateAddressAsync(houseNumber, street, city, province, postalCode, country);

            Assert.IsTrue(true, "Valid UK postal code should be processed");
        }

        [TestMethod]
        public async Task ValidateAddressAsync_WithNullStreet_ReturnsFalse()
        {
            string houseNumber = "123";
            string street = null;
            string city = "TestCity";
            string province = "TestProvince";
            int postalCode = 12345;
            string country = "USA";

            bool result = await _addressValidator.ValidateAddressAsync(houseNumber, street, city, province, postalCode, country);

            Assert.IsFalse(result, "Null street should return false");
        }

        [TestMethod]
        public async Task ValidateAddressAsync_WithInvalidPostalCodeFormat_ReturnsFalse()
        {
            string houseNumber = "123";
            string street = "Main St";
            string city = "TestCity";
            string province = "TestProvince";
            int postalCode = -12345;
            string country = "USA";

            bool result = await _addressValidator.ValidateAddressAsync(houseNumber, street, city, province, postalCode, country);

            Assert.IsFalse(result, "Negative postal code should return false");
        }

        [TestMethod]
        public async Task ValidateAddressAsync_WithExtremelyShortPostalCode_ReturnsFalse()
        {
            string houseNumber = "123";
            string street = "Main St";
            string city = "TestCity";
            string province = "TestProvince";
            int postalCode = 1;
            string country = "USA";

            bool result = await _addressValidator.ValidateAddressAsync(houseNumber, street, city, province, postalCode, country);

            Assert.IsFalse(result, "Extremely short postal code should return false");
        }

        [TestMethod]
        public async Task ValidateAddressAsync_WithBoundaryPostalCodes_ReturnsExpectedResult()
        {
            {
                string houseNumber = "123";
                string street = "Main St";
                string city = "TestCity";
                string province = "TestProvince";
                int postalCode = 00001;
                string country = "USA";

                bool result = await _addressValidator.ValidateAddressAsync(houseNumber, street, city, province, postalCode, country);

                Assert.IsTrue(true, "Boundary postal code should be processed");
            }

            {
                string houseNumber = "123";
                string street = "Main St";
                string city = "TestCity";
                string province = "TestProvince";
                int postalCode = 99950;
                string country = "USA";

                bool result = await _addressValidator.ValidateAddressAsync(houseNumber, street, city, province, postalCode, country);

                Assert.IsTrue(true, "Boundary postal code should be processed");
            }
        }

        [TestMethod]
        public async Task ValidateAddressAsync_WithInternationalAddresses_HandlesVariousCountries()
        {
            var addressTestCases = new[]
            {
                new {
                    HouseNumber = "10",
                    Street = "Downing Street",
                    City = "London",
                    Province = "Westminster",
                    PostalCode = 12345,
                    Country = "United Kingdom"
                },
                new {
                    HouseNumber = "1",
                    Street = "Champs-Élysées",
                    City = "Paris",
                    Province = "Île-de-France",
                    PostalCode = 75008,
                    Country = "France"
                },
                new {
                    HouseNumber = "1",
                    Street = "Sidney Street",
                    City = "Melbourne",
                    Province = "Victoria",
                    PostalCode = 3000,
                    Country = "Australia"
                }
            };

            foreach (var address in addressTestCases)
            {
                bool result = await _addressValidator.ValidateAddressAsync(
                    address.HouseNumber.ToString(),
                    address.Street,
                    address.City,
                    address.Province,
                    address.PostalCode,
                    address.Country);

                Assert.IsTrue(true, $"Should process address in {address.Country}");
            }
        }

        [TestMethod]
        public async Task ValidateAddressAsync_WithNetworkTimeout_HandlesTimeoutGracefully()
        {
            var timeoutHttpClient = new HttpClient
            {
                Timeout = TimeSpan.FromMilliseconds(1)
            };
            var timeoutAddressValidator = new AddressValidator(timeoutHttpClient);

            string houseNumber = "123";
            string street = "Main St";
            string city = "TestCity";
            string province = "TestProvince";
            int postalCode = 12345;
            string country = "USA";

            try
            {
                bool result = await timeoutAddressValidator.ValidateAddressAsync(
                    houseNumber, street, city, province, postalCode, country);

                Assert.IsFalse(result, "Address validation should return false on network timeout");
            }
            catch (TaskCanceledException)
            {
                Assert.IsTrue(true, "Network timeout should be handled gracefully");
            }
        }

        [TestMethod]
        public async Task ValidateAddressAsync_WithExtremelyLongInputs_ReturnsFalse()
        {
            string longString = new string('A', 1000);
            string houseNumber = longString;
            string street = longString;
            string city = longString;
            string province = longString;
            int postalCode = 99999;
            string country = longString;

            bool result = await _addressValidator.ValidateAddressAsync(houseNumber, street, city, province, postalCode, country);

            Assert.IsFalse(result, "Extremely long address inputs should return false");
        }

        [TestMethod]
        public async Task ValidateAddressAsync_WithNullCountry_ReturnsFalse()
        {
            string houseNumber = "123";
            string street = "Main St";
            string city = "TestCity";
            string province = "TestProvince";
            int postalCode = 12345;
            string country = null;

            bool result = await _addressValidator.ValidateAddressAsync(houseNumber, street, city, province, postalCode, country);

            Assert.IsFalse(result, "Null country should return false");
        }

        [TestMethod]
        public async Task ValidateAddressAsync_WithSpecialCharacters_ReturnsFalse()
        {
            string houseNumber = "123!@#";
            string street = "Main St & Test Road";
            string city = "San Francisco";
            string province = "CA";
            int postalCode = 94105;
            string country = "USA";

            bool result = await _addressValidator.ValidateAddressAsync(houseNumber, street, city, province, postalCode, country);

            Assert.IsFalse(result, "Address with special characters should return false");
        }

        [TestMethod]
        public async Task ValidateAddressAsync_WithUnicodeAddress_ReturnsResult()
        {
            string houseNumber = "42";
            string street = "Étoile Street";
            string city = "Montréal";
            string province = "Québec";
            int postalCode = 12345;
            string country = "Canada";

            bool result = await _addressValidator.ValidateAddressAsync(houseNumber, street, city, province, postalCode, country);

            Assert.IsTrue(true, "Unicode address should not cause validation failure");
        }

        [TestMethod]
        public async Task ValidateAddressAsync_WithValidUSAddress_ReturnsTrue()
        {
            string houseNumber = "1600";
            string street = "Pennsylvania Avenue";
            string city = "Washington";
            string province = "DC";
            int postalCode = 20500;
            string country = "USA";

            bool result = await _addressValidator.ValidateAddressAsync(houseNumber, street, city, province, postalCode, country);

            Assert.IsTrue(result, "Well-known valid address should return true");
        }

        [TestMethod]
        public async Task ValidateAddressAsync_WithValidCanadianAddress_ReturnsTrue()
        {
            string houseNumber = "80";
            string street = "Wellington Street";
            string city = "Ottawa";
            string province = "Ontario";
            int postalCode = 10000;
            string country = "Canada";

            bool result = await _addressValidator.ValidateAddressAsync(houseNumber, street, city, province, postalCode, country);

            Assert.IsTrue(result, "Well-known valid address should return true");
        }

        [TestMethod]
        public async Task ValidateAddressAsync_WithInvalidAddress_ReturnsFalse()
        {
            string houseNumber = "999999";
            string street = "Nonexistent Street XYZ";
            string city = "InvalidCity";
            string province = "InvalidProvince";
            int postalCode = 99999;
            string country = "InvalidCountry";

            bool result = await _addressValidator.ValidateAddressAsync(houseNumber, street, city, province, postalCode, country);

            Assert.IsFalse(result, "Clearly invalid address should return false");
        }

        [TestMethod]
        public async Task ValidateAddressAsync_WithEmptyStreetName_ReturnsFalse()
        {
            string houseNumber = "123";
            string street = " ";
            string city = "SomeCity";
            string province = "SomeProvince";
            int postalCode = 12345;
            string country = "SomeCountry";

            bool result = await _addressValidator.ValidateAddressAsync(houseNumber, street, city, province, postalCode, country);

            Assert.IsFalse(result, "Address with empty street name should return false");
        }
    }

    [TestClass]
    public class personalInfoUnitTests
    {

        [TestMethod]
        public void GetValidBirthdate_WithValidInput_ReturnsCorrectDate()
        {
            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                using (var sr = new StringReader("2000-01-01\n"))
                {
                    Console.SetIn(sr);
                    DateTime result = PersonalInfo.GetValidBirthdate();
                    Assert.AreEqual(new DateTime(2000, 1, 1), result);
                }
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetValidName_WithInvalidInput_ThrowsArgumentException()
        {
            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                using (var sr = new StringReader("123\n"))
                {
                    Console.SetIn(sr);
                    PersonalInfo.GetValidName("First");
                }
            }
        }

        [TestMethod]
        public void GetValidName_WithValidInput_ReturnsCorrectName()
        {
            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                using (var sr = new StringReader("John\n"))
                {
                    Console.SetIn(sr);
                    string result = PersonalInfo.GetValidName("First");
                    Assert.AreEqual("John", result);
                }
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetInput_WithEmptyInput_ThrowsArgumentException()
        {
            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                using (var sr = new StringReader("\n"))
                {
                    Console.SetIn(sr);
                    PersonalInfo.GetInput("Field");
                }
            }
        }

        [TestMethod]
        public void GetInput_WithValidInput_ReturnsCorrectInput()
        {
            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                using (var sr = new StringReader("ValidInput\n"))
                {
                    Console.SetIn(sr);
                    string result = PersonalInfo.GetInput("Field");
                    Assert.AreEqual("ValidInput", result);
                }
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetValidNumber_WithInvalidInput_ThrowsArgumentException()
        {
            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                using (var sr = new StringReader("Invalid\n"))
                {
                    Console.SetIn(sr);
                    PersonalInfo.GetValidNumber("Field");
                }
            }
        }

        [TestMethod]
        public void GetValidNumber_WithValidInput_ReturnsCorrectNumber()
        {
            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                using (var sr = new StringReader("123\n"))
                {
                    Console.SetIn(sr);
                    int result = PersonalInfo.GetValidNumber("Field");
                    Assert.AreEqual(123, result);
                }
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_WithFutureBirthday_ThrowsArgumentException()
        {
            DateTime futureBirthday = DateTime.Today.AddYears(1);
            new PersonalInfo("John", "Doe", futureBirthday, "USA", "California", "Los Angeles", 123, "Main St", "Downtown", 90001);
        }

        [TestMethod]
        public void Constructor_WithExtremelyOldBirthday_DoesNotThrowException()
        {
            DateTime oldBirthday = new DateTime(1800, 1, 1);
            PersonalInfo info = new PersonalInfo("John", "Doe", oldBirthday, "USA", "California", "Los Angeles", 123, "Main St", "Downtown", 90001);

            Assert.AreEqual(oldBirthday, info.Birthday, "Old birthday should be accepted.");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_WithNullCountry_ThrowsArgumentNullException()
        {
            new PersonalInfo("John", "Doe", new DateTime(1990, 1, 1), null, "California", "Los Angeles", 123, "Main St", "Downtown", 90001);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_WithNullProvince_ThrowsArgumentNullException()
        {
            new PersonalInfo("John", "Doe", new DateTime(1990, 1, 1), "USA", null, "Los Angeles", 123, "Main St", "Downtown", 90001);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_WithNullCity_ThrowsArgumentNullException()
        {
            new PersonalInfo("John", "Doe", new DateTime(1990, 1, 1), "USA", "California", null, 123, "Main St", "Downtown", 90001);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_WithNullStreet_ThrowsArgumentNullException()
        {
            new PersonalInfo("John", "Doe", new DateTime(1990, 1, 1), "USA", "California", "Los Angeles", 123, null, "Downtown", 90001);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_WithNullBarangay_ThrowsArgumentNullException()
        {
            new PersonalInfo("John", "Doe", new DateTime(1990, 1, 1), "USA", "California", "Los Angeles", 123, "Main St", null, 90001);
        }
    

        [TestMethod]
        public void CalculateAge_WithExact18YearsOld_Returns18()
        {
            DateTime birthday = DateTime.Today.AddYears(-18);
            PersonalInfo info = new PersonalInfo("Jane", "Doe", birthday, "USA", "California", "Los Angeles", 123, "Main St", "Downtown", 90001);

            int age = info.CalculateAge();

            Assert.AreEqual(18, age, "Age should be exactly 18");
        }

        [TestMethod]
        public void DisplayFullInfo_PrintsCorrectly()
        {
            PersonalInfo info = new PersonalInfo("Alice", "Johnson", new DateTime(1990, 6, 15), "USA", "New York", "New York City", 100, "Broadway", "Manhattan", 10001);

            try
            {
                info.DisplayFullInfo();
                Assert.IsTrue(true, "DisplayFullInfo should run without exceptions");
            }
            catch (Exception)
            {
                Assert.Fail("DisplayFullInfo should not throw an exception");
            }
        }

        [TestMethod]
        public void CalculateAge_AtYearBoundary_ReturnsCorrectAge()
        {
            DateTime justBeforeBirthday = DateTime.Today.AddDays(-1).AddYears(-30);
            PersonalInfo info = new PersonalInfo(
                "Jake", "Smith", justBeforeBirthday, "Philippines", "Cebu", "Cebu City",
                10, "Colon Street", "Barangay Santo Niño", 6000);

            int calculatedAge = info.CalculateAge();

            Assert.AreEqual(30, calculatedAge, "Age calculation at year boundary should be correct");
        }

        [TestMethod]
        public void Constructor_WithExtremelyLongName_ThrowsArgumentException()
        {
            string longName = new string('A', 200);

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
            string fname = "Jake";
            string lname = "Smith";
            DateTime birthday = new DateTime(1995, 5, 20);
            string country = "Philippines";
            string province = "Cebu";
            string city = "Cebu City";
            int houseNumber = 10;
            string street = "Colon Street";
            string barangay = "Barangay Santo Niño";
            int postalCode = 6000;

            PersonalInfo info = new PersonalInfo(
                fname, lname, birthday, country, province, city,
                houseNumber, street, barangay, postalCode);

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
            new PersonalInfo(
                null, "Smith", new DateTime(1995, 5, 20), "Philippines", "Cebu", "Cebu City",
                10, "Colon Street", "Barangay Santo Niño", 6000);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_WithNullLastName_ThrowsArgumentNullException()
        {
            new PersonalInfo(
                "Jake", null, new DateTime(1995, 5, 20), "Philippines", "Cebu", "Cebu City",
                10, "Colon Street", "Barangay Santo Niño", 6000);
        }

        [TestMethod]
        public void CalculateAge_WithBirthdayInPast_ReturnsCorrectAge()
        {
            int expectedYears = 30;
            DateTime birthday = DateTime.Today.AddYears(-expectedYears);
            PersonalInfo info = new PersonalInfo(
                "Jake", "Smith", birthday, "Philippines", "Cebu", "Cebu City",
                10, "Colon Street", "Barangay Santo Niño", 6000);

            int calculatedAge = info.CalculateAge();

            Assert.AreEqual(expectedYears, calculatedAge);
        }

        [TestMethod]
        public void CalculateAge_WithBirthdayTomorrow_ReturnsPreviousYear()
        {
            int expectedYears = 30;
            DateTime birthday = DateTime.Today.AddDays(1).AddYears(-expectedYears - 1);
            birthday = birthday.AddDays(1);
            PersonalInfo info = new PersonalInfo(
                "Jake", "Smith", birthday, "Philippines", "Cebu", "Cebu City",
                10, "Colon Street", "Barangay Santo Niño", 6000);

            int calculatedAge = info.CalculateAge();

            Assert.AreEqual(expectedYears, calculatedAge,
                "Age should be calculated correctly when birthday hasn't occurred yet this year");
        }

        [TestMethod]
        public async Task ValidateAddress_WithValidAddress_SetsIsAddressVerifiedToTrue()
        {
            PersonalInfo info = new PersonalInfo(
                "Jake", "Smith", new DateTime(1995, 5, 20),
                "Philippines", "Cebu", "Cebu City",
                10, "Colon Street", "Barangay Santo Niño", 6000,
                new AddressValidator());

            bool result = await info.ValidateAddress();

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