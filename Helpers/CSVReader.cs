using CSVReaderTask.EF;
using CSVReaderTask.Helpers.Interfaces;
using CSVReaderTask.Models;
using CsvHelper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CsvHelper.Configuration;
using System.Windows;
using CsvHelper.TypeConversion;
using CSVReaderTask.Models.CsvMap;

namespace CSVReaderTask.Helpers
{
    public class CSVReader : ICSVReader
    {
        private readonly CSVContext _dbContext;
        const int BUNCH_SIZE = 30;

        public CSVReader(CSVContext dbContext)
        {
            _dbContext = dbContext;
        }
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
                            await _dbContext.AddRangeAsync(bunch);
                            await _dbContext.SaveChangesAsync();
                            bunch.Clear();
                        }
                    }
                    await transaction.CommitAsync();
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
