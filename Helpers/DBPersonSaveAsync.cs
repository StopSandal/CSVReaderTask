using CsvHelper.TypeConversion;
using CSVReaderTask.EF;
using CSVReaderTask.Helpers.Interfaces;
using CSVReaderTask.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CSVReaderTask.Helpers
{
    public class DBPersonSaveAsync : IDBPersonSaveAsync
    {
        private readonly CSVContext _dbContext;
        private const int BunchSize = 30;
        public DBPersonSaveAsync(CSVContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> SavePersonsToDB(IAsyncEnumerable<Person> peoples)
        {
            var bunch = new List<Person>();
            int recordsCount = 0;
            await using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                await foreach (var record in peoples)
                {
                    recordsCount++;
                    bunch.Add(record);
                    if (bunch.Count > BunchSize)
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
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
            return recordsCount;
        }
    }
}
