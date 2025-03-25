using AddressValidatorLibrary;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Basic_information_library.Models
{
    public class PersonalInfo
    {
        private readonly AddressValidator _addressValidator;

        // Properties
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

        public PersonalInfo(
            string fname,
            string lname,
            DateTime birthday,
            string country,
            string province,
            string city,
            int houseNumber,
            string street,
            string barangay,
            int postalCode,
            AddressValidator addressValidator = null)
        {
            if (fname == null) throw new ArgumentNullException(nameof(fname));
            if (lname == null) throw new ArgumentNullException(nameof(lname));
            if (fname.Length > 60) throw new ArgumentException("First name is too long", nameof(fname));
            if (lname.Length > 60) throw new ArgumentException("Last name is too long", nameof(lname));

            Fname = fname;
            Lname = lname;
            Birthday = birthday;
            Country = country ?? throw new ArgumentNullException(nameof(country));
            Province = province ?? throw new ArgumentNullException(nameof(province));
            City = city ?? throw new ArgumentNullException(nameof(city));
            HouseNumber = houseNumber;
            Street = street ?? throw new ArgumentNullException(nameof(street));
            Barangay = barangay ?? throw new ArgumentNullException(nameof(barangay));
            PostalCode = postalCode;
            IsAddressVerified = false;
            _addressValidator = addressValidator ?? new AddressValidator();
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

        public static DateTime GetValidBirthdate()
        {
            DateTime birthdate;
            while (true)
            {
                Console.Write("Enter your birthdate (YYYY-MM-DD): ");
                string input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine($"{RedText}Error:{ResetText} Birthdate cannot be empty. Please enter a valid date.");
                    continue;
                }

                if (DateTime.TryParseExact(input, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out birthdate))
                {
                    if (birthdate > DateTime.Now)
                    {
                        Console.WriteLine($"{RedText}Error:{ResetText} Birthdate cannot be in the future. Please enter a valid date in the format YYYY-MM-DD.");
                    }
                    else if (birthdate < new DateTime(1900, 1, 1))
                    {
                        Console.WriteLine($"{RedText}Error:{ResetText} Birthdate cannot be earlier than 1900. Please enter a valid date in the format YYYY-MM-DD.");
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

        public static string GetValidName(string nameType)
        {
            string name;
            do
            {
                Console.Write($"Enter your {nameType} name: ");
                name = Console.ReadLine()?.Trim();  // Trim extra spaces to prevent empty input issues

                if (string.IsNullOrWhiteSpace(name))
                {
                    Console.WriteLine($"{RedText}Error:{ResetText} Name cannot be empty. Please enter a valid name.");
                    continue;
                }

                if (!name.All(c => char.IsLetter(c) || char.IsWhiteSpace(c)))
                {
                    Console.WriteLine($"{RedText}Error:{ResetText} Invalid name. Please enter a valid name (only letters and spaces).");
                }
                else if (name.Length > 60)
                {
                    Console.WriteLine($"{RedText}Error:{ResetText} Name is too long. Enter less than 60 characters.");
                }
            } while (string.IsNullOrWhiteSpace(name) ||
                    !name.All(c => char.IsLetter(c) || char.IsWhiteSpace(c)) ||
                    name.Length > 60);

            return name;
        }

        public static string GetInput(string fieldName)
        {
            string input;
            do
            {
                Console.Write($"{fieldName}: ");
                input = Console.ReadLine()?.Trim();
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

                    if (string.IsNullOrWhiteSpace(input))
                    {
                        Console.WriteLine($"{RedText}Error:{ResetText} {fieldName} cannot be empty. Please enter a valid number.");
                        continue;
                    }

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

        public int CalculateAge()
        {
            int age = DateTime.Today.Year - Birthday.Year;
            //if birthday hasn't happen yet this year, subtract 1 from age
            if (DateTime.Today < Birthday.AddYears(age))
            {
                age--;
            }
            return age;
        }

        public async Task<bool> ValidateAddress()
        {
            bool isValid = await _addressValidator.ValidateAddressAsync(
                HouseNumber.ToString(),
                Street,
                City,
                Province,
                PostalCode,
                Country);

            IsAddressVerified = isValid;
            return isValid;
        }
    }
}