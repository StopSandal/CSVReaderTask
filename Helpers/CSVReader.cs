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

            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csv.GetRecordsAsync<Person>();
                var bunch = new List<Person>();
                await using var transaction = await _dbContext.Database.BeginTransactionAsync();
                try
                {
                    await foreach (var record in records)
                    {
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
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
                
            }
        }

    }
}
