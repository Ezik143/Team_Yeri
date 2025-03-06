using Information;
using System;
class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("NAME");
        Console.WriteLine();

        string fname, lname;
        do
        {
            Console.Write("Enter your first name: ");
            fname = Console.ReadLine();
            Console.Write("Enter your last name: ");
            lname = Console.ReadLine();
            if (!PersonalInfo.IsValidName(fname, lname))
            {
                Console.WriteLine("Invalid name. Please enter valid names (only alphabetic characters, no spaces or numbers).");
            }
        } while (!PersonalInfo.IsValidName(fname, lname));

        Console.WriteLine();
        Console.WriteLine("BIRTHDATE");
        Console.WriteLine();

        int bYear, bMonth, bDay;


        while (true)
        {
            Console.Write("Enter your birth year: ");
            if (int.TryParse(Console.ReadLine(), out bYear))
            {
                break;
            }
            Console.WriteLine("Invalid year. Please enter a valid year.");
        }


        while (true)
        {
            Console.Write("Enter your birth month (1-12): ");
            if (int.TryParse(Console.ReadLine(), out bMonth) && bMonth >= 1 && bMonth <= 12)
            {
                break;
            }
            Console.WriteLine("Invalid month. Please enter a valid month (1-12).");
        }


        while (true)
        {
            Console.Write("Enter your birthday (1-31): ");
            if (int.TryParse(Console.ReadLine(), out bDay) && bDay >= 1 && bDay <= 31)
            {

                if (PersonalInfo.IsValidDay(bYear, bMonth, bDay))
                {
                    break;
                }
                Console.WriteLine("Invalid day for the given month and year. Please enter a valid day.");
            }
            else
            {
                Console.WriteLine("Invalid day. Please enter a valid day (1-31).");
            }
        }

        PersonalInfo personalInfo = new PersonalInfo(fname, lname, new DateTime(bYear, bMonth, bDay), "", "", "", 0, "", "", 0);

        Console.WriteLine($"Your age is: {personalInfo.CalculateAge()}");
        Console.WriteLine();

        Console.WriteLine("ADDRESS");
        Console.WriteLine();

        Console.Write("Country: ");
        string country = Console.ReadLine();
        Console.Write("Province: ");
        string province = Console.ReadLine();
        Console.Write("City: ");
        string city = Console.ReadLine();
        Console.Write("House Number: ");
        if (!int.TryParse(Console.ReadLine(), out int houseNumber))
        {
            Console.WriteLine("Invalid house number.");
            return;
        }
        Console.Write("Street: ");
        string street = Console.ReadLine();
        Console.Write("Barangay: ");
        string barangay = Console.ReadLine();
        Console.Write("Postal Code: ");
        if (!int.TryParse(Console.ReadLine(), out int postalCode))
        {
            Console.WriteLine("Invalid postal code.");
            return;
        }
        personalInfo.DisplayFullName();
        personalInfo.DisplayAddress();
    }
}
