using CSVReaderTask.EF;
using CSVReaderTask.Helpers.Interfaces;
using CSVReaderTask.Models;
using CsvHelper;
using System.Globalization;
using System.IO;
using CsvHelper.Configuration;
using System.Windows;
using CsvHelper.TypeConversion;
using CSVReaderTask.Models.CsvMap;
using System.Reflection;

namespace CSVReaderTask.Helpers
{
    /// <summary>
    /// Provides functionality to read a CSV file and save its contents to a database using Entity Framework Core.
    /// Implements <see cref="ICSVReader"/>.
    /// </summary>
    public class CSVReader : ICSVReader
    {

        private const string CSVDelimiter = ";";

        /// <summary>
        /// Initializes a new instance of the <see cref="CSVReader"/> class.
        /// </summary>
        public CSVReader()
        {
        }

        /// <inheritdoc />
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="filePath"/> is null or empty.</exception>
        /// <exception cref="FileNotFoundException">Thrown when the specified <paramref name="filePath"/> does not exist.</exception>
        /// <exception cref="IOException">Thrown when an error occurs during file reading.</exception>
        public Task<IAsyncEnumerable<Person>> ReadFilePersonAsync(string filePath)
        {
            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    HasHeaderRecord = false,
                    Delimiter = CSVDelimiter,
                    BadDataFound = context =>
                    {
                        throw new Exception($"Bad data found : {context.RawRecord}");
                    },
                    MissingFieldFound = context =>
                    {
                        throw new Exception($"Missing field  on row { context.Index }");
                    }
                }))
            {
                csv.Context.RegisterClassMap<PersonMap>();

                return Task.FromResult(csv.GetRecordsAsync<Person>());

            }
        }

    }
}
