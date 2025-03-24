using MySql.Data.MySqlClient;
using MySql.Data.MySqlClient;
using Information;
using System;
public class database
{
    public static void SaveToDatabase(PersonalInfo info)
    {
        // Set this to your MySQL password before running the program or set it as an environment variable.
        string password = Environment.GetEnvironmentVariable("MYSQL_PASSWORD");
        string connectionString = $"server=localhost;database=basicinformationdb;user=root;password={password};";
        using (MySqlConnection conn = new MySqlConnection(connectionString))
        {
            conn.Open();

            string query = @"INSERT INTO PersonalInfo (FirstName, LastName, Birthdate, Country, Province, City, HouseNumber, Street, Barangay, PostalCode, IsAddressVerified) 
                             VALUES (@FirstName, @LastName, @Birthdate, @Country, @Province, @City, @HouseNumber, @Street, @Barangay, @PostalCode, @IsAddressVerified)";

            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@FirstName", info.Fname);
                cmd.Parameters.AddWithValue("@LastName", info.Lname);
                cmd.Parameters.AddWithValue("@Birthdate", info.Birthday);
                cmd.Parameters.AddWithValue("@Country", info.Country);
                cmd.Parameters.AddWithValue("@Province", info.Province);
                cmd.Parameters.AddWithValue("@City", info.City);
                cmd.Parameters.AddWithValue("@HouseNumber", info.HouseNumber);
                cmd.Parameters.AddWithValue("@Street", info.Street);
                cmd.Parameters.AddWithValue("@Barangay", info.Barangay);
                cmd.Parameters.AddWithValue("@PostalCode", info.PostalCode);
                cmd.Parameters.AddWithValue("@IsAddressVerified", info.IsAddressVerified);
                cmd.ExecuteNonQuery();
            }

            Console.WriteLine("Data successfully saved to the database!");
        }
    }
}
