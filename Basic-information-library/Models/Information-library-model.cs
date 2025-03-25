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
            if (birthday > DateTime.Today)
                throw new ArgumentException("Birthday cannot be in the future", nameof(birthday));

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
            // Validate name length
            if (nameType == null)
                throw new ArgumentNullException(nameof(nameType), "Name type cannot be null");

            while (true)
            {
                string name = Console.ReadLine()?.Trim();

                if (string.IsNullOrWhiteSpace(name))
                    throw new ArgumentException($"{nameType} name cannot be empty.");

                if (!name.All(c => char.IsLetter(c) || char.IsWhiteSpace(c)))
                    throw new ArgumentException($"Invalid {nameType} name. Only letters and spaces are allowed.");

                if (name.Length > 60)
                    throw new ArgumentException($"{nameType} name must be less than 60 characters.");

                return name;
            }
        }


        public static string GetInput(string fieldName)
        {
            if (fieldName == null)
                throw new ArgumentNullException(nameof(fieldName), "Field name cannot be null");

            string input = Console.ReadLine()?.Trim();

            if (string.IsNullOrWhiteSpace(input))
                throw new ArgumentException($"{fieldName} cannot be empty.");

            return input;
        }


        public static int GetValidNumber(string fieldName)
        {
            if (fieldName == null)
                throw new ArgumentNullException(nameof(fieldName), "Field name cannot be null");

            string input = Console.ReadLine()?.Trim();

            if (string.IsNullOrWhiteSpace(input))
                throw new ArgumentException($"{fieldName} cannot be empty.");

            if (!int.TryParse(input, out int number) || number <= 0)
                throw new ArgumentException($"Invalid {fieldName}. Please enter a valid positive number.");

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