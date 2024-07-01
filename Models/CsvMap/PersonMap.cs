using CsvHelper.Configuration;

namespace CSVReaderTask.Models.CsvMap
{
    /// <summary>
    /// CsvHelper mapping configuration for the Person class.
    /// </summary>
    public class PersonMap : ClassMap<Person>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PersonMap"/> class.
        /// Configures mapping for fields of the Person class.
        /// </summary>
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
