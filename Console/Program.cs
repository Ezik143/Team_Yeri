using System;
using System.Threading.Tasks;
using Basic_information_library.Models;
using AddressValidatorLibrary;
using System.Net.Http;
using Basic_information_library.Interfaces;

class Program
{
    const string GreenText = "\x1b[32m";
    const string ResetText = "\x1b[0m";
    const string CyanText = "\x1b[36m";

    static async Task Main(string[] args)
    {
        // Adding a timeout for HTTP client to prevent hanging
        var httpClientHandler = new HttpClientHandler();
        var httpClient = new HttpClient(httpClientHandler)
        {
            Timeout = TimeSpan.FromSeconds(30)
        };
        IPersonalInfoRepository repository = new PersonalInfoRepository();

        try
        {
            Console.WriteLine($"{CyanText}======================================{ResetText}");
            Console.WriteLine($"{CyanText}   PERSONAL INFORMATION SYSTEM      {ResetText}");
            Console.WriteLine($"{CyanText}======================================{ResetText}");

            // Get user's personal information
            string firstName = PersonalInfo.GetValidName("first");
            string lastName = PersonalInfo.GetValidName("last");
            DateTime birthdate = PersonalInfo.GetValidBirthdate();

            Console.WriteLine($"\n{CyanText}--- Address Information ---{ResetText}");
            string country = PersonalInfo.GetInput("Country");
            string province = PersonalInfo.GetInput("Province/State");
            string city = PersonalInfo.GetInput("City");
            string barangay = PersonalInfo.GetInput("Barangay/Subdivision");
            string street = PersonalInfo.GetInput("Street");
            int houseNumber = PersonalInfo.GetValidNumber("House Number");
            int postalCode = PersonalInfo.GetValidNumber("Postal Code");

            // Create an address validator with our configured HTTP client
            var addressValidator = new AddressValidator(httpClient);

            // Create a new PersonalInfo object
            var personalInfo = new PersonalInfo(
                firstName,
                lastName,
                birthdate,
                country,
                province,
                city,
                houseNumber,
                street,
                barangay,
                postalCode,
                addressValidator);

            // Attempt to validate the address
            Console.WriteLine($"\n{CyanText}Validating address...{ResetText}");

            try
            {
                bool isValid = await personalInfo.ValidateAddress();

                if (isValid)
                {
                    Console.WriteLine($"{GreenText}Address validation successful!{ResetText}");
                }
                else
                {
                    Console.WriteLine($"{PersonalInfo.YellowText}Address could not be validated. It may not exist or there might be an error in your input.{ResetText}");
                }
            }
            catch (TaskCanceledException)
            {
                Console.WriteLine($"{PersonalInfo.YellowText}Address validation timed out. Please check your internet connection.{ResetText}");
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"{PersonalInfo.YellowText}Network error during address validation: {ex.Message}{ResetText}");
            }

            // Update the display with the verification status
            personalInfo.DisplayFullInfo();

            // Save to database immediately without asking
            Console.WriteLine($"\n{CyanText}Saving information to database...{ResetText}");
            repository.Save(personalInfo);

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
        finally
        {
            httpClient.Dispose();
        }
    }
}