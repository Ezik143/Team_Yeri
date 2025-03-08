using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;
using System.IO;
using System.Text;
using System.Web;

namespace Information.Tests
{
    [TestClass]
    public class PersonalInfoTests
    {
        private PersonalInfo _person;

        [TestInitialize]
        public void Setup()
        {
            _person = new PersonalInfo("Jake", "Sucgang", new DateTime(2004, 12, 10),
                "Philippines", "Cebu", "Cebu City", 0, "Sitio San Miguel Street", "Tisa", 6000);
        }

        [TestMethod]
        public void CalculateAge_ShouldReturnCorrectAge()
        {
            // Act
            int age = _person.CalculateAge();

            // Assert
            int expectedAge = DateTime.Today.Year - 2004;
            if (DateTime.Today < new DateTime(2004, 12, 10).AddYears(expectedAge))
            {
                expectedAge--;
            }

            Assert.AreEqual(expectedAge, age);
        }

        [DataTestMethod]
        [DataRow("Jake", "Sucgang", true)]
        [DataRow("J@ke", "Sucgang", false)]
        [DataRow("Jake", "Sucg4ng", false)]
        [DataRow("", "Sucgang", false)]
        [DataRow("Jake", "", false)]
        public void IsValidName_ShouldValidateCorrectly(string fname, string lname, bool expected)
        {
            // Act
            bool result = PersonalInfo.IsValidName(fname, lname);

            // Assert
            Assert.AreEqual(expected, result);
        }

        [DataTestMethod]
        [DataRow(2004, 12, 10, true)] // Valid birthdate
        [DataRow(2024, 2, 29, false)] // Invalid leap year date
        [DataRow(2020, 2, 29, true)]  // Valid leap year date
        [DataRow(2023, 4, 31, false)] // Invalid April 31st
        public void IsValidDay_ShouldReturnCorrectValidity(int year, int month, int day, bool expected)
        {
            // Act
            bool result = PersonalInfo.IsValidDay(year, month, day);

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public async Task ValidateAddress_ShouldReturnTrue_WhenValidAddress()
        {
            // Arrange
            var fakeResponse = "[{\"address\": {\"road\": \"Sitio San Miguel Street\", \"city\": \"Cebu City\", \"state\": \"Cebu\", \"country\": \"Philippines\", \"postcode\": \"6000\"}}]";
            var fakeHttpMessageHandler = new FakeHttpMessageHandler(fakeResponse, HttpStatusCode.OK);
            var fakeHttpClient = new HttpClient(fakeHttpMessageHandler);

            // Inject the fake HttpClient into the PersonalInfo class
            typeof(PersonalInfo).GetField("client", BindingFlags.NonPublic | BindingFlags.Static).SetValue(null, fakeHttpClient);

            // Act
            bool isVerified = await _person.ValidateAddress();

            // Assert
            Assert.IsTrue(isVerified);
            Assert.IsTrue(_person.IsAddressVerified);
        }

        [TestMethod]
        public async Task ValidateAddress_ShouldReturnFalse_WhenInvalidAddress()
        {
            // Arrange
            var fakeHttpMessageHandler = new FakeHttpMessageHandler("[]", HttpStatusCode.OK);
            var fakeHttpClient = new HttpClient(fakeHttpMessageHandler);

            // Inject the fake HttpClient into the PersonalInfo class
            typeof(PersonalInfo).GetField("client", BindingFlags.NonPublic | BindingFlags.Static).SetValue(null, fakeHttpClient);

            // Act
            bool isVerified = await _person.ValidateAddress();

            // Assert
            Assert.IsFalse(isVerified);
            Assert.IsFalse(_person.IsAddressVerified);
        }
    }

    /// <summary>
    /// Fake HttpMessageHandler to mock API responses for testing without actually calling the API.
    /// </summary>
    public class FakeHttpMessageHandler : HttpMessageHandler
    {
        private readonly string _responseContent;
        private readonly HttpStatusCode _statusCode;

        public FakeHttpMessageHandler(string responseContent, HttpStatusCode statusCode)
        {
            _responseContent = responseContent;
            _statusCode = statusCode;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            return Task.FromResult(new HttpResponseMessage
            {
                StatusCode = _statusCode,
                Content = new StringContent(_responseContent, Encoding.UTF8, "application/json")
            });
        }
    }
}
