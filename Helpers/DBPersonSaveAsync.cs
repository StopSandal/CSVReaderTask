using CsvHelper.TypeConversion;
using CSVReaderTask.EF;
using CSVReaderTask.Helpers.Interfaces;
using CSVReaderTask.Models;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CSVReaderTask.Helpers
{
    public class DBPersonSaveAsync : IDBPersonSaveAsync
    {
        private readonly CSVContext _dbContext;

        public DBPersonSaveAsync(CSVContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> SavePersonsToDBAsync(IEnumerable<Person> peoples)
        {
            try
            {
                await _dbContext.BulkInsertAsync(peoples);

            }
            catch
            {
                throw;
            }
            return peoples.Count();
        }
    }
}
