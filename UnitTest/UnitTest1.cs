using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace Information.Tests
{
    [TestClass]
    public class PersonalInfoTests
    {
        [TestMethod]
        public void GetValidName_ValidName_ReturnsName()
        {
            string validName = "Jake";
            bool result = PersonalInfo.IsValidName(validName, "Sucgang");
            Assert.IsTrue(result, "Valid name should be accepted.");
        }

        [TestMethod]
        public void GetValidName_InvalidName_ReturnsFalse()
        {
            string invalidName = "Jake6000";
            bool result = PersonalInfo.IsValidName(invalidName, "Sucgang");
            Assert.IsFalse(result, "Invalid name should be rejected.");
        }

        [TestMethod]
        public void GetValidBirthdate_ValidDate_ReturnsDate()
        {
            DateTime birthdate = new DateTime(2004, 12, 10);
            PersonalInfo person = new PersonalInfo("Jake", "Sucgang", birthdate, "Philippines", "Cebu", "Cebu City", 123, "Colon Street", "Tisa", 6000);
            Assert.AreEqual(birthdate, person.Birthday, "Birthdate should be correctly assigned.");
        }

        [TestMethod]
        public void CalculateAge_CorrectAge_ReturnsExpectedAge()
        {
            DateTime birthdate = new DateTime(2000, 1, 1);
            PersonalInfo person = new PersonalInfo("Jake", "Sucgang", birthdate, "Philippines", "Cebu", "Cebu City", 123, "Colon Street", "Tisa", 6000);
            int expectedAge = DateTime.Now.Year - birthdate.Year;
            if (DateTime.Now < birthdate.AddYears(expectedAge)) expectedAge--;
            Assert.AreEqual(expectedAge, person.CalculateAge(), "Age calculation should be correct.");
        }

        [TestMethod]
        public async Task ValidateAddress_ReturnsFalseOnInvalidAddress()
        {
            PersonalInfo person = new PersonalInfo("Jake", "Sucgang", new DateTime(1990, 1, 1), "FakeCountry", "FakeProvince", "FakeCity", 0, "FakeStreet", "FakeBarangay", 99999);
            bool result = await person.ValidateAddress();
            Assert.IsFalse(result, "Invalid address should not be verified.");
        }
    }
}
