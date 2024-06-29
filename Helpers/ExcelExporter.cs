using CSVReaderTask.Helpers.Interfaces;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CSVReaderTask.Helpers
{
    public class ExcelExporter : IExcelExport
    {

        async Task IFileExport.ExportFileAsync<TClass>(string filePath, IEnumerable<TClass> dataCollection) 
        {
            try
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                using (var package = new ExcelPackage(new FileInfo(filePath)))
                {

                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("New 1");

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
                            worksheet.Cells[row, i + 1].Value = properties[i].GetValue(item);
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
    }
}
