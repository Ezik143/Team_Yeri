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

        public void DisplayFullName()
        {
            Console.WriteLine($"Your full name is: {Fname} {Lname}");
        }

        public static bool IsValidName(string fname, string lname)
        {
            return !string.IsNullOrEmpty(fname) && !string.IsNullOrEmpty(lname) &&
                   fname.All(char.IsLetter) && lname.All(char.IsLetter);
        }

        public static bool IsValidDay(int year, int month, int day)
        {
            if (month == 2)
            {
                if (DateTime.IsLeapYear(year))
                {
                    return day <= 29;
                }
                return day <= 28;
            }


            if (month == 4 || month == 6 || month == 9 || month == 11)
            {
                return day <= 30;
            }

            return day <= 31;
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

        public void DisplayAddress()
        {
            Console.WriteLine($"Your complete address is: {HouseNumber} {Street}, {Barangay}, {City}, {Province} {PostalCode}, {Country}");
        }

    }
}