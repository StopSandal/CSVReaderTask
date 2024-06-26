using CSVReaderTask.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSVReaderTask.EF
{
    public class CSVContext : DbContext
    {
        internal DbSet<Person> Persons { get; set; }
        public CSVContext(DbContextOptions<CSVContext> options) : base(options)
        {
        }
    }
}
