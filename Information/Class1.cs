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
        }

        public void DisplayFullInfo()
        {
            Console.WriteLine($"Your full name is: {Fname} {Lname}");
            Console.WriteLine($"Your age is: {CalculateAge()}");
            Console.WriteLine($"Your complete address is: {HouseNumber} {Street}, {Barangay}, {City}, {Province}, {PostalCode}, {Country}");
        }

        public static string GetValidName(string nameType)
        {
            const string RedText = "\x1b[31m";
            const string ResetText = "\x1b[0m";
            string name;
            do
            {
                Console.Write($"Enter your {nameType} name: ");
                name = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(name) || !name.All(char.IsLetter))
                {
                    Console.WriteLine($"{RedText}Error:{ResetText} Invalid name. Please enter a valid name (only alphabetic characters, no spaces or numbers).");
                }
            } while (string.IsNullOrWhiteSpace(name) || !name.All(char.IsLetter));
            return name;
        }

        public static DateTime GetValidBirthdate()
        {
            const string RedText = "\x1b[31m";
            const string ResetText = "\x1b[0m";
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
            const string RedText = "\x1b[31m";
            const string ResetText = "\x1b[0m";
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
            const string RedText = "\x1b[31m";
            const string ResetText = "\x1b[0m";
            int number;
            while (true)
            {
                Console.Write($"{fieldName}: ");
                string input = Console.ReadLine();
                if (int.TryParse(input, out number) && number > 0)
                {
                    break;
                }
                Console.WriteLine($"{RedText}Error:{ResetText} Invalid {fieldName}. Please enter a valid positive number.");
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
    }
}