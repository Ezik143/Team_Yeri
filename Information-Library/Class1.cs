using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using System.Linq;

namespace Information
{
    public class PersonalInfo
    {
        public string Fname { get; set; }
        public string Lname { get; set; }
        public DateTime Birthday { get; set; }
        public string Country { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public int HouseNumber { get; set; }
        public string Street { get; set; }
        public string Barangay { get; set; }
        public int PostalCode { get; set; }
        public bool IsAddressVerified { get; set; }

        public const string RedText = "\x1b[31m";
        public const string GreenText = "\x1b[32m";
        public const string YellowText = "\x1b[33m";
        public const string BlueText = "\x1b[34m";
        public const string ResetText = "\x1b[0m";

        private static readonly HttpClient client = new HttpClient();
        private const string GeocodingApiUrl = "https://nominatim.openstreetmap.org/search";

        static PersonalInfo()
        {
            // Set user agent to comply with OSM Nominatim usage policy
            client.DefaultRequestHeaders.Add("User-Agent", "PersonalInfoApp/1.0");
        }

        public PersonalInfo(string fname, string lname, DateTime birthday, string country, string province, string city, int houseNumber, string street, string barangay, int postalCode)
        {
            Fname = fname;
            Lname = lname;
            Birthday = birthday;
            Country = country;
            Province = province;
            City = city;
            HouseNumber = houseNumber;
            Street = street;
            Barangay = barangay;
            PostalCode = postalCode;
            IsAddressVerified = false;
        }

        public void DisplayFullInfo()
        {
            Console.WriteLine($"\n{BlueText}===== PERSONAL INFORMATION ====={ResetText}");
            Console.WriteLine($"Your full name is: {Fname} {Lname}");
            Console.WriteLine($"Your age is: {CalculateAge()}");
            Console.WriteLine($"Your complete address is: {HouseNumber} {Street}, {Barangay}, {City}, {Province}, {PostalCode}, {Country}");

            if (IsAddressVerified)
                Console.WriteLine($"{GreenText}Address Status: Verified ✓{ResetText}");
            else
                Console.WriteLine($"{YellowText}Address Status: Not Verified{ResetText}");
        }

        public static string GetValidName(string nameType)
        {
            string name;
            do
            {
                Console.Write($"Enter your {nameType} name: ");
                name = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(name) || !name.All(char.IsLetter))
                {
                    Console.WriteLine($"{RedText}Error:{ResetText} Invalid name. Please enter a valid name (only alphabetic characters, no spaces or numbers).");
                }
                else if (name.Length > 60)
                {
                    Console.WriteLine($"{RedText}Error:{ResetText} Name is too long. Please enter a name with less than 60 characters.");
                }
            } while (string.IsNullOrWhiteSpace(name) || !name.All(char.IsLetter) || name.Length > 60);
            return name;
        }

        public static DateTime GetValidBirthdate()
        {
            DateTime birthdate;
            while (true)
            {
                Console.Write("Enter your birthdate (YYYY-MM-DD): ");
                string input = Console.ReadLine();
                if (DateTime.TryParse(input, out birthdate))
                {
                    if (birthdate > DateTime.Now)
                    {
                        Console.WriteLine($"{RedText}Error:{ResetText} Birthdate cannot be in the future. Please enter a valid date.");
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    Console.WriteLine($"{RedText}Error:{ResetText} Invalid date format. Please enter a valid date in the format YYYY-MM-DD.");
                }
            }
            return birthdate;
        }

        public static string GetInput(string fieldName)
        {
            string input;
            do
            {
                Console.Write($"{fieldName}: ");
                input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine($"{RedText}Error:{ResetText} {fieldName} cannot be empty. Please enter a valid value.");
                }
            } while (string.IsNullOrWhiteSpace(input));
            return input;
        }

        public static int GetValidNumber(string fieldName)
        {
            int number;
            while (true)
            {
                try
                {
                    Console.Write($"{fieldName}: ");
                    string input = Console.ReadLine();
                    if (int.TryParse(input, out number) && number > 0)
                    {
                        break;
                    }
                    Console.WriteLine($"{RedText}Error:{ResetText} Invalid {fieldName}. Please enter a valid positive number.");
                }
                catch (OverflowException)
                {
                    Console.WriteLine($"{RedText}Error:{ResetText} You inputted a number that is too large.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"{RedText}Error:{ResetText} Unexpected error occurred: {ex.Message}");
                }
            }
            return number;
        }

        public static bool IsValidName(string fname, string lname)
        {
            return !string.IsNullOrEmpty(fname) && !string.IsNullOrEmpty(lname) &&
                   fname.All(char.IsLetter) && lname.All(char.IsLetter);
        }

        public static bool IsValidDay(int year, int month, int day)
        {
            return day <= DateTime.DaysInMonth(year, month);
        }

        public int CalculateAge()
        {
            int age = DateTime.Today.Year - Birthday.Year;
            if (DateTime.Today < Birthday.AddYears(age))
            {
                age--;
            }
            return age;
        }

        public async Task<bool> ValidateAddress()
        {
            try
            {
                Console.WriteLine($"\n{BlueText}Validating address...{ResetText}");

                // Build the address query string
                string addressQuery = $"{HouseNumber} {Street}, {City}, {Province}, {PostalCode}, {Country}";

                // Create URL with parameters
                string requestUrl = $"{GeocodingApiUrl}?q={Uri.EscapeDataString(addressQuery)}&format=json&addressdetails=1&limit=1";

                // Send the request
                HttpResponseMessage response = await client.GetAsync(requestUrl);
                response.EnsureSuccessStatusCode();

                // Get the response content
                string responseBody = await response.Content.ReadAsStringAsync();

                // Parse JSON response
                using (JsonDocument doc = JsonDocument.Parse(responseBody))
                {
                    JsonElement root = doc.RootElement;

                    // Check if any results were returned
                    if (root.GetArrayLength() > 0)
                    {
                        Console.WriteLine($"{GreenText}Address validation successful!{ResetText}");
                        Console.WriteLine("\nFound address details:");

                        JsonElement result = root[0];

                        // Display address components if available
                        if (result.TryGetProperty("address", out JsonElement address))
                        {
                            DisplayAddressComponent(address, "road", "Street");
                            DisplayAddressComponent(address, "city", "City");
                            DisplayAddressComponent(address, "state", "State/Province");
                            DisplayAddressComponent(address, "country", "Country");
                            DisplayAddressComponent(address, "postcode", "Postal Code");
                        }

                        IsAddressVerified = true;
                        return true;
                    }

                    Console.WriteLine($"{YellowText}Warning:{ResetText} The address could not be verified. It may not exist or there might be spelling errors.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{RedText}Error validating address:{ResetText} {ex.Message}");
                return false;
            }
        }

        private static void DisplayAddressComponent(JsonElement address, string key, string label)
        {
            if (address.TryGetProperty(key, out JsonElement value))
            {
                Console.WriteLine($"{label}: {value}");
            }
        }
    }
}
