using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Information;  
using MydbContext;
using System.Data.Entity;
using MySql.Data.MySqlClient;

namespace UnitTest
{
    public class DBHELPER
    {
        private const string ConnectionString = "server=localhost;database=mydb;user=root;password=D0min1c;";

        public static MySqlConnection GetConnection()
        {
            MySqlConnection conn = new MySqlConnection(ConnectionString);
            conn.Open();
            return conn;
        }

        public static bool InsertPersonalInfo(PersonalInfo person)
        {
            using (var context = new MyDbContext()) 
            {
                context.PersonalInfos.Add(person);
                context.SaveChanges(); 
                return true;
            }
        }
    }
}
