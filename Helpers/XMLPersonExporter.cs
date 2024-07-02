using CSVReaderTask.Helpers.Interfaces;
using CSVReaderTask.Models;
using System.Reflection;
using System.Windows;
using System.Xml;

namespace CSVReaderTask.Helpers
{
    /// <summary>
    /// Provides methods to export data to XML files.
    /// Implements <see cref="IXMLPersonExport"/>.
    /// </summary>
    public class XMLPersonExporter : IXMLPersonExport
    {
        private const string NullPrefix = null;
        private const string IdColumnName = "Id";
        private const string RecordElementName = "Record";
        private readonly string AppName = Application.ResourceAssembly.GetName().Name ?? "App";

        /// <inheritdoc />
        /// <exception cref="Exception">Thrown when an error occurs during XML export.</exception>
        public async Task ExportFileAsync<TClass>(string filePath, IEnumerable<TClass> dataCollection)
        {
            try
            {
                var settings = new XmlWriterSettings()
                {
                    Async = true,
                    Indent = true
                };

                using (XmlWriter writer = XmlWriter.Create(filePath, settings))
                {
                    await writer.WriteStartDocumentAsync();
                    await writer.WriteStartElementAsync(NullPrefix, AppName, NullPrefix);

                    foreach (var item in dataCollection)
                    {
                        PropertyInfo[] properties = typeof(TClass).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                        await writer.WriteStartElementAsync(NullPrefix, RecordElementName, NullPrefix);
                        foreach (var property in properties)
                        {
                            if (property.Name == IdColumnName)
                                continue;
                            await writer.WriteStartElementAsync(NullPrefix, property.Name, NullPrefix);
                            var value = property.GetValue(item);
                            await writer.WriteStringAsync(value?.ToString() ?? string.Empty);
                            await writer.WriteEndElementAsync();
                        }
                        await writer.WriteEndElementAsync();
                    }

                    await writer.WriteEndElementAsync();
                    await writer.WriteEndDocumentAsync();
                }

            }
            catch
            {
                 throw;
            }
        }

        /// <inheritdoc />
        /// <exception cref="Exception">Thrown when an error occurs during XML export.</exception>
        public async Task ExportPersonsFileAsync<TClass>(string filePath, IEnumerable<TClass> dataCollection) where TClass : Person
        {
            try
            {
                var settings = new XmlWriterSettings()
                {
                    Async = true,
                    Indent = true
                };

                using (XmlWriter writer = XmlWriter.Create(filePath, settings))
                {
                    await writer.WriteStartDocumentAsync();
                    await writer.WriteStartElementAsync(NullPrefix, AppName, NullPrefix);

                    foreach (var person in dataCollection)
                    {
                        await writer.WriteStartElementAsync(NullPrefix, RecordElementName, NullPrefix);
                        await writer.WriteAttributeStringAsync(NullPrefix, "id", NullPrefix, person.Id.ToString());

                        PropertyInfo[] properties = typeof(Person).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                        foreach (var property in properties)
                        {
                            if (property.Name == IdColumnName)
                                continue;
                            await writer.WriteStartElementAsync(NullPrefix, property.Name, NullPrefix);
                            var value = property.GetValue(person);
                            await writer.WriteStringAsync(value?.ToString() ?? string.Empty);
                            await writer.WriteEndElementAsync();
                        }

                        await writer.WriteEndElementAsync();
                    }

                    await writer.WriteEndElementAsync();
                    await writer.WriteEndDocumentAsync();
                }

            }
            catch
            {
                 throw;
            }
        }
    }
}
