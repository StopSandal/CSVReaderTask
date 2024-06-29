using CSVReaderTask.Helpers.Interfaces;
using CSVReaderTask.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;

namespace CSVReaderTask.Helpers
{
    public class XMLPersonExporter : IXMLPersonExport
    {
        private const string NULL_PREFIX = null;
        private const string ID_COLUMN_NAME = "Id";
        private const string RECORD_ELEMENT_NAME = "Record";
        
        public async Task ExportFileAsync<TClass>(string filePath, IEnumerable<TClass> dataCollection) 
        {
            try
            {
                XmlWriterSettings settings = new XmlWriterSettings()
                {
                    Async = true,
                    Indent = true
                };

                using (XmlWriter writer = XmlWriter.Create(filePath, settings))
                {
                    await writer.WriteStartDocumentAsync();
                    await writer.WriteStartElementAsync(NULL_PREFIX,Application.ResourceAssembly.GetName().Name, NULL_PREFIX);

                    foreach (var item in dataCollection)
                    {
                        PropertyInfo[] properties = typeof(TClass).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                        await writer.WriteStartElementAsync(NULL_PREFIX, RECORD_ELEMENT_NAME, NULL_PREFIX);
                        foreach (var property in properties)
                        {
                            if (property.Name == ID_COLUMN_NAME)
                                continue;
                            await writer.WriteStartElementAsync(NULL_PREFIX,property.Name, NULL_PREFIX);
                            var value = property.GetValue(item);
                            await writer.WriteStringAsync(value?.ToString() ?? string.Empty);
                            await writer.WriteEndElementAsync();
                        }
                        await writer.WriteEndElementAsync();
                    }

                    await writer.WriteEndElementAsync();
                    await writer.WriteEndDocumentAsync();
                }

                MessageBox.Show("Export to XML successful.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error exporting to XML: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public async Task ExportPersonsFileAsync<TClass>(string filePath, IEnumerable<TClass> dataCollection) where TClass : Person
        {
            try
            {
                XmlWriterSettings settings = new XmlWriterSettings()
                {
                    Async = true,
                    Indent = true
                };

                using (XmlWriter writer = XmlWriter.Create(filePath, settings))
                {
                    await writer.WriteStartDocumentAsync();
                    await writer.WriteStartElementAsync(NULL_PREFIX,Application.ResourceAssembly.GetName().Name, NULL_PREFIX);

                    foreach (var person in dataCollection)
                    {
                        await writer.WriteStartElementAsync(NULL_PREFIX, RECORD_ELEMENT_NAME, NULL_PREFIX);
                        await writer.WriteAttributeStringAsync(NULL_PREFIX,"id", NULL_PREFIX, person.Id.ToString()); 

                        PropertyInfo[] properties = typeof(Person).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                        foreach (var property in properties)
                        {
                            if (property.Name == ID_COLUMN_NAME)
                                continue;
                            await writer.WriteStartElementAsync(NULL_PREFIX,property.Name, NULL_PREFIX);
                            var value = property.GetValue(person);
                            await writer.WriteStringAsync(value?.ToString() ?? string.Empty);
                            await writer.WriteEndElementAsync();
                        }

                        await writer.WriteEndElementAsync();
                    }

                    await writer.WriteEndElementAsync();
                    await writer.WriteEndDocumentAsync();
                }

                MessageBox.Show("Export to XML successful.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error exporting to XML: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
