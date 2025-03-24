using Microsoft.VisualStudio.TestTools.UnitTesting;
using Basic_information_library.Models;
using System;
using System.Data.Entity;
using Database = Basic_information_library.Models.Database;

namespace UnitTest
{
    [TestClass]
    public class UnitTest3
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
