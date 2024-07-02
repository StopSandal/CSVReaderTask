using CSVReaderTask.Models;
using Microsoft.EntityFrameworkCore;

namespace CSVReaderTask.EF
{
    public class CSVContext : DbContext
    {
        internal DbSet<Person> Persons { get; set; }
        public CSVContext(DbContextOptions<CSVContext> options) : base(options)
        {
            Database.Migrate();
        }
    }
}
