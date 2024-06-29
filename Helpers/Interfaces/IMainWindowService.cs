using CSVReaderTask.Models;
using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSVReaderTask.Helpers.Interfaces
{
    public interface IMainWindowService
    {
        public Task ReadCSVFileAsync(string filePath);

        public Task SavePersonInfoToExcelAsync(string filePath, IEnumerable<Person> dataCollection);
        public Task SavePersonInfoToXMLAsync(string filePath, IEnumerable<Person> dataCollection);
    }
}
