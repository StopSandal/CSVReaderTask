﻿using CSVReaderTask.Helpers.Interfaces;
using CSVReaderTask.Models;
using System.Diagnostics;

namespace CSVReaderTask.Helpers
{
    /// <summary>
    /// Service class that coordinates interactions between UI and data export functionalities.
    /// Implements <see cref="IMainWindowService"/>.
    /// </summary>
    public class MainWindowService : IMainWindowService
    {
        private readonly ICSVReader _csvReader;
        private readonly IExcelExport _excelExport;
        private readonly IXMLPersonExport _personExport;
        private readonly IDBPersonSaveAsync _personSaveAsync;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindowService"/> class with dependencies.
        /// </summary>
        /// <param name="csvReader">The CSV reader service to read and save CSV data to the database.</param>
        /// <param name="excelExport">The Excel export service to export data to Excel files.</param>
        /// <param name="personExport">The XML person export service to export person data to XML files.</param>
        public MainWindowService(ICSVReader csvReader, IExcelExport excelExport, IXMLPersonExport personExport, IDBPersonSaveAsync personSaveAsync)
        {
            _csvReader = csvReader;
            _excelExport = excelExport;
            _personExport = personExport;
            _personSaveAsync = personSaveAsync;
        }

        /// <inheritdoc />
        public async Task<int> ReadCSVFileAsync(string filePath)
        {
            var time = DateTime.Now;
            var peoples = _csvReader.ReadFilePersonAsync(filePath);
            int recordNumber = 0;

            await foreach (var people in peoples)
                recordNumber += await _personSaveAsync.SavePersonsToDBAsync(people);

            var timeAfter = DateTime.Now;
            Debug.WriteLine($"Time for csv read file {timeAfter - time}");
            return recordNumber;
        }

        /// <inheritdoc />
        public async Task SavePersonInfoToExcelAsync(string filePath, IAsyncEnumerable<Person> dataCollection)
        {
            await _excelExport.ExportFileAsync(filePath, dataCollection);
        }

        /// <inheritdoc />
        public async Task SavePersonInfoToXMLAsync(string filePath, IAsyncEnumerable<Person> dataCollection)
        {
            await _personExport.ExportPersonsFileAsync(filePath, dataCollection);
        }
    }
}
