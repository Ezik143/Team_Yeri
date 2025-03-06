using System;
using Information;

class Program
{
    const string GreenText = "\x1b[32m";
    const string ResetText = "\x1b[0m";

    static void Main(string[] args)
    {
        try
        {
            Console.WriteLine($"{GreenText}NAME{ResetText}");
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
            personalInfo.DisplayFullInfo();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{GreenText}Error:{ResetText} An unexpected error occurred: {ex.Message}");
        }
    }
}