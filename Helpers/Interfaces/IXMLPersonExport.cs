using CSVReaderTask.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSVReaderTask.Helpers.Interfaces
{
    public interface IXMLPersonExport : IXMLExport
    {
        public Task ExportPersonsFileAsync<TClass>(string filePath, IEnumerable<TClass> dataCollection) where TClass : Person;
    }
}
