using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using MySql.Data.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Information;


namespace MydbContext
{
    [DbConfigurationType(typeof(MySqlEFConfiguration))] // MySQL-specific EF configuration
    public class MyDbContext : DbContext
    {
        public MyDbContext() : base("MyDbContext") // Matches App.config name
        {
        }

        public DbSet<PersonalInfo> PersonalInfos { get; set; } // Table in the database

        public void SaveChanges()
        {
            throw new NotImplementedException();
        }
    }
}

