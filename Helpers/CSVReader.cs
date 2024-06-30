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

namespace CSVReaderTask.Helpers
{
    /// <summary>
    /// Provides functionality to read a CSV file and save its contents to a database using Entity Framework Core.
    /// Implements <see cref="ICSVReader"/>.
    /// </summary>
    public class CSVReader : ICSVReader
    {
        private readonly CSVContext _dbContext;
        private const int BUNCH_SIZE = 30;

        /// <summary>
        /// Initializes a new instance of the <see cref="CSVReader"/> class with the specified database context.
        /// </summary>
        /// <param name="dbContext">The database context to use for database operations.</param>
        public CSVReader(CSVContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <inheritdoc />
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="filePath"/> is null or empty.</exception>
        /// <exception cref="FileNotFoundException">Thrown when the specified <paramref name="filePath"/> does not exist.</exception>
        /// <exception cref="IOException">Thrown when an error occurs during file reading.</exception>
        /// <exception cref="TypeConverterException">Thrown when there is an error converting data types from the CSV file.</exception>
        public async Task ReadFileAndSaveToDBAsync(string filePath)
        {
            int recordsCount = 0;
            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    HasHeaderRecord = false,
                    Delimiter = ";",
                    BadDataFound = context =>
                    {
                        System.Windows.MessageBox.Show($"Bad data found : {context.RawRecord}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        throw new Exception("Bad data");
                    },
                    MissingFieldFound = context =>
                    {
                        System.Windows.MessageBox.Show($"Missing field  on row {context.Index}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        throw new Exception("Bad data");
                    }
                }))
            {
                csv.Context.RegisterClassMap<PersonMap>();

                var records = csv.GetRecordsAsync<Person>();

                var bunch = new List<Person>();
                await using var transaction = await _dbContext.Database.BeginTransactionAsync();
                try
                {
                    await foreach (var record in records)
                    {
                        recordsCount++;
                        bunch.Add(record);
                        if (bunch.Count > BUNCH_SIZE)
                        {
                            await _dbContext.Persons.AddRangeAsync(bunch);
                            await _dbContext.SaveChangesAsync();
                            bunch.Clear();
                        }
                    }
                    await transaction.CommitAsync();
                    await _dbContext.Persons.AddRangeAsync(bunch);
                    await _dbContext.SaveChangesAsync();
                }
                catch (TypeConverterException ex)
                {
                    System.Windows.MessageBox.Show($"Unable to read data field, check your file", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    throw;
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
                
            }
            
            System.Windows.MessageBox.Show($"File was successfully read. Total records {recordsCount}", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }

    }
}
