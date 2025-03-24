using Information;
using MydbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersoninfoRepo
{
    public class PersonalInfoRepository
    {
        private readonly MyDbContext _context;

        //Inject MyDbContext through constructor
        public PersonalInfoRepository(MyDbContext context)
        {
            _context = context;
        }

        // Method to add a new person
        public void AddPersonalInfo(PersonalInfo person)
        {
            _context.PersonalInfos.Add(person);
            _context.SaveChanges();
        }

        // Method to get all records
        public List<PersonalInfo> GetAllPersonalInfo()
        {
            return _context.PersonalInfos.ToList();
        }
    }
}
