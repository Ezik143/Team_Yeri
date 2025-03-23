using System;
using System.Threading.Tasks;
using Information;
using AddressValidatorLibrary;
using System.Net.Http;
using Database;

class Program
{
    const string GreenText = "\x1b[32m";
    const string ResetText = "\x1b[0m";
    const string CyanText = "\x1b[36m";

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
            DatabaseManager.SaveToDatabase(personalInfo);

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
}