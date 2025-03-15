using System.Threading.Tasks;
using System;
using Information;
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

            PersonalInfo personalInfo = new PersonalInfo(fname, lname, birthdate, country, province, city, houseNumber, street, barangay, postalCode);

            Console.WriteLine();
            Console.Write("Would you like to validate your address? (Y/N): ");
            string validateChoice = Console.ReadLine().Trim().ToUpper();

            if (validateChoice == "Y")
            {
                await personalInfo.ValidateAddress();
            }

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