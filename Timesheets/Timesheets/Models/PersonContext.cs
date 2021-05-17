using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Timesheets.Models
{
    public class PersonContext : DbContext
    {
        public DbSet<Person> Personsss { get; set; }
    }
}
