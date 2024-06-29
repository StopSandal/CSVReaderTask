using CSVReaderTask.Helpers.Interfaces;
using CSVReaderTask.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSVReaderTask.Helpers
{
    public class MainWindowService : IMainWindowService
    {
        private readonly ICSVReader _csvReader;
        private readonly IExcelExport _excelExport;
        private readonly IXMLPersonExport _personExport;

        public MainWindowService(ICSVReader csvReader, IExcelExport excelExport, IXMLPersonExport personExport)
        {
            _csvReader = csvReader;
            _excelExport = excelExport;
            _personExport = personExport;
        }

        public async Task ReadCSVFileAsync(string filePath)
        {
            await _csvReader.ReadFileAndSaveToDBAsync(filePath);
        }

        public async Task SavePersonInfoToExcelAsync(string filePath, IEnumerable<Person> dataCollection)
        {
            await _excelExport.ExportFileAsync(filePath, dataCollection);
        }

        public async Task SavePersonInfoToXMLAsync(string filePath, IEnumerable<Person> dataCollection)
        {
            await _personExport.ExportPersonsFileAsync(filePath, dataCollection);
        }
    }
}
