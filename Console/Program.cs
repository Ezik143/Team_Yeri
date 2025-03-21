﻿using System;
using System.Threading.Tasks; //for async and task
using MySql.Data.MySqlClient;// for MySQL
using Information; // for PersonalInfo
using AddressValidatorLibrary;// for AddressValidator
using System.Net.Http; // for HttpClient to send HTTP requests

class Program
{
    const string GreenText = "\x1b[32m";
    const string ResetText = "\x1b[0m";
    const string CyanText = "\x1b[36m";


    //async for the await keyword and short for asynchronous 
    //task for the return type while waiting for the task to complete(anything that has await)
    static async Task Main(string[] args)
    {
        try
        {
            Console.WriteLine($"{CyanText}======================================{ResetText}");
            Console.WriteLine($"{CyanText}   PERSONAL INFORMATION SYSTEM      {ResetText}");
            Console.WriteLine($"{CyanText}======================================{ResetText}");

            // Get Personal Information from User
            Console.WriteLine($"\n{GreenText}NAME{ResetText}");
            Console.WriteLine();
            string fname = PersonalInfo.GetValidName("first");
            string lname = PersonalInfo.GetValidName("last");

            Console.WriteLine();
            Console.WriteLine($"{GreenText}BIRTHDATE{ResetText}");
            Console.WriteLine();
            DateTime birthdate = PersonalInfo.GetValidBirthdate();

            Console.WriteLine();
            Console.WriteLine($"{GreenText}ADDRESS{ResetText}");
            Console.WriteLine();
            string country = PersonalInfo.GetInput("Country");
            string province = PersonalInfo.GetInput("Province");
            string city = PersonalInfo.GetInput("City");
            int houseNumber = PersonalInfo.GetValidNumber("House Number");
            string street = PersonalInfo.GetInput("Street");
            string barangay = PersonalInfo.GetInput("Barangay");
            int postalCode = PersonalInfo.GetValidNumber("Postal Code");

            // Create HttpClient with appropriate timeout
            var httpClient = new HttpClient();
            httpClient.Timeout = TimeSpan.FromSeconds(10);

            // Create services with DI
            IAddressValidator addressValidator = new AddressValidator(httpClient);

            PersonalInfo personalInfo = new PersonalInfo(
                fname, lname, birthdate, country, province, city,
                houseNumber, street, barangay, postalCode,
                addressValidator);

            await personalInfo.ValidateAddress();
            

            // Save to MySQL Database
            SaveToDatabase(personalInfo);

            personalInfo.DisplayFullInfo();

            Console.WriteLine($"\n{CyanText}======================================{ResetText}");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{PersonalInfo.RedText}Error:{ResetText} An unexpected error occurred: {ex.Message}");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }

    static void SaveToDatabase(PersonalInfo info)
    {
        //set this to your MySQL password before running the program or set it as an environment variable hehehe.
        string password = Environment.GetEnvironmentVariable("MYSQL_PASSWORD");
        string connectionString = $"server=localhost;database=basicinformationdb;user=root;password={password};";
        using (MySqlConnection conn = new MySqlConnection(connectionString))
        {
            conn.Open();

            string query = @"INSERT INTO PersonalInfo (FirstName, LastName, Birthdate, Country, Province, City, HouseNumber, Street, Barangay, PostalCode, IsAddressVerified) 
                             VALUES (@FirstName, @LastName, @Birthdate, @Country, @Province, @City, @HouseNumber, @Street, @Barangay, @PostalCode,@IsAddressVerified)";

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

            Console.WriteLine($"{GreenText}Data successfully saved to the database!{ResetText}");
        }
    }
}