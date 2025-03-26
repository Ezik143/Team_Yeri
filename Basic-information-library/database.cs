using System;
using System.Data.SqlClient;
using Basic_information_library.Models;
using Basic_information_library.Interfaces;

namespace Basic_information_library.Models
{
    public static class Database
    {
        public static void SaveToDatabase(PersonalInfo info)
        {
            var repository = new PersonalInfoRepository();
            repository.Save(info);
        }
    }

    public class PersonalInfoRepository : IPersonalInfoRepository
    {
        private readonly string _connectionString;

        public PersonalInfoRepository()
        {
            // Retrieve the connection string from a secure location
            string password = Environment.GetEnvironmentVariable("MYSQL_PASSWORD");
            if (string.IsNullOrEmpty(password))
            {
                throw new InvalidOperationException("Error: MySQL password not set");
            }

            // Construct the connection string
            _connectionString = $"Server=your_server;Database=basicinformationdb;User Id=root;Password={password};";
        }

        public void Save(PersonalInfo info)
        {
            if (info == null)
            {
                throw new ArgumentNullException(nameof(info), "PersonalInfo cannot be null.");
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    string query = "INSERT INTO PersonalInfo (Fname, Lname, Birthday, Country, Province, City, HouseNumber, Street, Barangay, PostalCode) " +
                                "VALUES (@Fname, @Lname, @Birthday, @Country, @Province, @City, @HouseNumber, @Street, @Barangay, @PostalCode)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Fname", info.Fname);
                        command.Parameters.AddWithValue("@Lname", info.Lname);
                        command.Parameters.AddWithValue("@Birthday", info.Birthday);
                        command.Parameters.AddWithValue("@Country", info.Country);
                        command.Parameters.AddWithValue("@Province", info.Province);
                        command.Parameters.AddWithValue("@City", info.City);
                        command.Parameters.AddWithValue("@HouseNumber", info.HouseNumber);
                        command.Parameters.AddWithValue("@Street", info.Street);
                        command.Parameters.AddWithValue("@Barangay", info.Barangay);
                        command.Parameters.AddWithValue("@PostalCode", info.PostalCode);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex)
            {
                // Handle specific database exceptions
                throw new Exception("Database Error: " + ex.Message);
            }
        }
    }
}