using System;
using System.Linq;
using System.Threading.Tasks;
using AddressValidatorLibrary;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Castle.Components.DictionaryAdapter;


namespace Information
{
    [Table("personalinfo")]
    public class PersonalInfo
    {
        private readonly IAddressValidator _addressValidator;
        // Properties
        [System.ComponentModel.DataAnnotations.Key]
        public int Id { get; set; }
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
            IAddressValidator addressValidator = null)
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
                if (DateTime.TryParse(input, out birthdate))
                {
                    if (birthdate > DateTime.Now)
                    {
                        Console.WriteLine($"{RedText}Error:{ResetText} Birthdate cannot be in the future. Please enter a valid date.");
                    }
                    else if (birthdate < new DateTime(1900, 1, 1))
                    {
                        Console.WriteLine($"{RedText}Error{ResetText}: Birthdate cannot be earlier than 1900. Please enter a valid date.");
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

                if (string.IsNullOrWhiteSpace(name) || !name.All(char.IsLetter))
                {
                    Console.WriteLine("Error: Invalid name. Please enter a valid name (only letters).");
                }
                else if (name.Length > 60)
                {
                    Console.WriteLine("Error: Name is too long. Enter less than 60 characters.");
                }
            } while (string.IsNullOrWhiteSpace(name) || !name.All(char.IsLetter) || name.Length > 60);

            return name;
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