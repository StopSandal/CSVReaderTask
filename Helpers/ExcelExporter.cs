using CSVReaderTask.Helpers.Interfaces;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CSVReaderTask.Helpers
{
    /// <summary>
    /// Provides functionality to export data to an Excel file using EPPlus.
    /// Implements <see cref="IExcelExport"/>.
    /// </summary>
    public class ExcelExporter : IExcelExport
    {
        private const string SHEET_NAME = $"New";
        private const string EXPORT_DATE_FORMAT = "dd.mm.yyyy";
        /// <inheritdoc />
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="filePath"/> is null or empty.</exception>
        /// <exception cref="IOException">Thrown when an error occurs during file reading or writing.</exception>
        async Task IFileExport.ExportFileAsync<TClass>(string filePath, IEnumerable<TClass> dataCollection) 
        {
            try
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                using (var package = new ExcelPackage(new FileInfo(filePath)))
                {
                    ExcelWorksheet worksheet = CreateNewSheet(package);

                    PropertyInfo[] properties = typeof(TClass).GetProperties();

                    for (int i = 0; i < properties.Length; i++)
                    {
                        worksheet.Cells[1, i + 1].Value = properties[i].Name;
                    }

                    int row = 2;
                    foreach (var item in dataCollection)
                    {
                        for (int i = 0; i < properties.Length; i++)
                        {
                            var value = properties[i].GetValue(item);

                            if (value is DateTime dateValue)
                            {
                                worksheet.Cells[row, i + 1].Value = dateValue.ToString(EXPORT_DATE_FORMAT);
                            }
                            else
                            {
                                worksheet.Cells[row, i + 1].Value = value;
                            }
                        }
                        row++;
                    }

                    await package.SaveAsync();
                }

                MessageBox.Show("Export to Excel successful.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error exporting to Excel: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        /// <summary>
        /// Creates new sheet with unique name
        /// </summary>
        /// <param name="excelPackage">Instance of package <see cref="ExcelPackage"/></param>
        /// <returns>Unique worksheet <see cref="ExcelWorksheet"/></returns>
        public ExcelWorksheet CreateNewSheet(ExcelPackage excelPackage)
        {
            if (excelPackage.Workbook.Worksheets.Any(ws => ws.Name == SHEET_NAME))
            {
                int count = 1;
                string uniqueName = $"{SHEET_NAME}_{count}";

                while (excelPackage.Workbook.Worksheets.Any(ws => ws.Name == uniqueName))
                {
                    count++;
                    uniqueName = $"{SHEET_NAME}_{count}";
                }

                return excelPackage.Workbook.Worksheets.Add(uniqueName);
            }
            else
            {
                return excelPackage.Workbook.Worksheets.Add(SHEET_NAME);
            }
        }
    }
}
