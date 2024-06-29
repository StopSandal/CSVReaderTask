using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSVReaderTask.Models.CsvMap
{
    public class PersonMap : ClassMap<Person>
    {
        public PersonMap()
        {
            Map(m => m.Date).Index(0).TypeConverterOption.Format("dd.MM.yyyy");
            Map(m => m.FirstName).Index(1).Validate(field => !string.IsNullOrWhiteSpace(field.Field));
            Map(m => m.LastName).Index(2).Validate(field => !string.IsNullOrWhiteSpace(field.Field));
            Map(m => m.SurName).Index(3).Validate(field => !string.IsNullOrWhiteSpace(field.Field)); ;
            Map(m => m.City).Index(4).Validate(field => !string.IsNullOrWhiteSpace(field.Field)); ;
            Map(m => m.Country).Index(5).Validate(field => !string.IsNullOrWhiteSpace(field.Field)); ;
        }
    }
}
