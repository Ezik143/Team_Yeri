using System.Threading.Tasks;
using System;

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
            string fname = Information.PersonalInfo.GetValidName("first");
            string lname = Information.PersonalInfo.GetValidName("last");

            Console.WriteLine();
            Console.WriteLine($"{GreenText}BIRTHDATE{ResetText}");
            Console.WriteLine();
            DateTime birthdate = Information.PersonalInfo.GetValidBirthdate();

            Console.WriteLine();
            Console.WriteLine($"{GreenText}ADDRESS{ResetText}");
            Console.WriteLine();
            string country = Information.PersonalInfo.GetInput("Country");
            string province = Information.PersonalInfo.GetInput("Province");
            string city = Information.PersonalInfo.GetInput("City");
            int houseNumber = Information.PersonalInfo.GetValidNumber("House Number");
            string street = Information.PersonalInfo.GetInput("Street");
            string barangay = Information.PersonalInfo.GetInput("Barangay");
            int postalCode = Information.PersonalInfo.GetValidNumber("Postal Code");

            Information.PersonalInfo personalInfo = new Information.PersonalInfo(fname, lname, birthdate, country, province, city, houseNumber, street, barangay, postalCode);

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
            Console.WriteLine($"{Information.PersonalInfo.RedText}Error:{ResetText} An unexpected error occurred: {ex.Message}");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}