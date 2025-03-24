using System;
using System.IO;// for stringwriter and stringreader
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Information;
using AddressValidatorLibrary;

namespace UnitTest
{
    [TestClass]
    public class PersonalInfoTests
    {
        // INPUT VALIDATION TESTS 
        [TestMethod]
        public void GetValidName_ValidInput_ReturnsName()
        {
            string input = "John\n";  // Ensure newline to simulate user pressing Enter
            using (StringReader sr = new StringReader(input))
            using (StringWriter sw = new StringWriter())
            {
                Console.SetIn(sr);
                Console.SetOut(sw);

                string result = PersonalInfo.GetValidName("first");

                Assert.AreEqual("John", result);
            }
        }

        [TestMethod]
        public void GetValidName_InvalidInput_ShowsErrorAndRetries()
        {
            string input = "\n123\nJohn\n"; // Empty, then invalid (123), then valid (John)
            using (StringReader sr = new StringReader(input))
            using (StringWriter sw = new StringWriter())
            {
                Console.SetIn(sr);
                Console.SetOut(sw);

                string result = PersonalInfo.GetValidName("first");

                Assert.AreEqual("John", result);

                string output = sw.ToString();

                Assert.IsTrue(output.Contains("Error: Invalid name"),
                    $"Expected error message not found in: {output}");
            }
        }

        [TestMethod]
        public void GetValidBirthdate_ValidInput_ReturnsDate()
        {
            string input = "2000-05-10";
            using (StringReader sr = new StringReader(input))
            using (StringWriter sw = new StringWriter())
            {
                Console.SetIn(sr);
                Console.SetOut(sw);
                DateTime result = PersonalInfo.GetValidBirthdate();
                Assert.AreEqual(new DateTime(2000, 5, 10), result);
            }
        }

        [TestMethod]
        public void GetValidNumber_ValidInput_ReturnsNumber()
        {
            string input = "123";
            using (StringReader sr = new StringReader(input))
            using (StringWriter sw = new StringWriter())
            {
                Console.SetIn(sr);
                Console.SetOut(sw);
                int result = PersonalInfo.GetValidNumber("Age");
                Assert.AreEqual(123, result);
            }
        }



        // AGE CALCULATION TEST 

        [TestMethod]
        public void CalculateAge_ReturnsCorrectAge()
        {
            DateTime birthdate = new DateTime(1995, 8, 15);
            PersonalInfo person = new PersonalInfo("Jake", "Smith", birthdate, "Philippines", "Cebu", "Cebu", 456, "Mango Avenue", "Barangay Lahug", 6000);
            int age = person.CalculateAge();
            int expectedAge = DateTime.Today.Year - 1995;
            if (DateTime.Today < new DateTime(DateTime.Today.Year, 8, 15))
                expectedAge--;
            Assert.AreEqual(expectedAge, age);
        }

 

        [TestMethod]
        public async Task ValidateAddress_RealAPI_ReturnsExpectedResult()
        {
            // Arrange: Use real HttpClient to call the API
            HttpClient client = new HttpClient();
            AddressValidator realValidator = new AddressValidator(client);

            // Sample real-world address
            bool isValid = await realValidator.ValidateAddressAsync(
                "1600", "Amphitheatre Parkway", "Mountain View", "California", 94043, "USA");

            // Assert: Ensure response is valid
            Assert.IsTrue(isValid, "API should return true for a real address.");
        }

        [TestMethod]
        public async Task ValidateAddress_RealAPI_InvalidAddress_ReturnsFalse()
        {
            // Arrange
            HttpClient client = new HttpClient();
            AddressValidator realValidator = new AddressValidator(client);

            // A non-existent address
            bool isValid = await realValidator.ValidateAddressAsync(
                "999999", "Fake Street XYZ", "Nowhere", "Imaginary", 99999, "Neverland");

            // Assert
            Assert.IsFalse(isValid, "API should return false for an invalid address.");
        }

        [TestMethod]
        public async Task ValidateAddress_HandlesExceptionGracefully()
        {
            // Arrange
            HttpClient client = new HttpClient();
            AddressValidator realValidator = new AddressValidator(client);

            // Inject an invalid BaseAddress to force an error
            typeof(AddressValidator).GetField("_client", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .SetValue(realValidator, new HttpClient { BaseAddress = new Uri("https://invalid-url.com") });

            bool result = await realValidator.ValidateAddressAsync("1600", "Amphitheatre Parkway", "Mountain View", "California", 94043, "USA");

            // Assert
            Assert.IsFalse(result, "API should return false when a network failure occurs.");
        }

        // CONSOLE OUTPUT TEST 

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
