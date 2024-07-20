using CSVReaderTask.Helpers.Interfaces;
using OfficeOpenXml;
using System.IO;
using System.Reflection;

namespace CSVReaderTask.Helpers
{
    /// <summary>
    /// Provides functionality to export data to an Excel file using EPPlus.
    /// Implements <see cref="IExcelExport"/>.
    /// </summary>
    public class ExcelExporter : IExcelExport
    {
        private const string SheetName = $"New";
        private const string ExportDateFormat = "dd.mm.yyyy";
        /// <inheritdoc />
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="filePath"/> is null or empty.</exception>
        /// <exception cref="IOException">Thrown when an error occurs during file reading or writing.</exception>
        async Task IFileExport.ExportFileAsync<TClass>(string filePath, IAsyncEnumerable<TClass> dataCollection)
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
                    await foreach (var item in dataCollection)
                          {
                              for (int i = 0; i < properties.Length; i++)
                              {
                                  var value = properties[i].GetValue(item);
                                try
                                {
                                    if (value is DateTime dateValue)
                                    {
                                        worksheet.Cells[row, i + 1].Value = dateValue.ToString(ExportDateFormat);
                                    }
                                    else
                                    {
                                        worksheet.Cells[row, i + 1].Value = value;
                                    }
                                }
                                catch(IndexOutOfRangeException ex)
                                {
                                    throw new Exception("TO much data to export. Export Canceled");
                                }

                              }
                        row++;
                          }

                    await package.SaveAsync();
                }

            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// Creates new sheet with unique name
        /// </summary>
        /// <param name="excelPackage">Instance of package <see cref="ExcelPackage"/></param>
        /// <returns>Unique worksheet <see cref="ExcelWorksheet"/></returns>
        private ExcelWorksheet CreateNewSheet(ExcelPackage excelPackage)
        {
            if (excelPackage.Workbook.Worksheets.Any(ws => ws.Name == SheetName))
            {
                int count = 1;
                string uniqueName = $"{SheetName}_{count}";

                while (excelPackage.Workbook.Worksheets.Any(ws => ws.Name == uniqueName))
                {
                    count++;
                    uniqueName = $"{SheetName}_{count}";
                }

                return excelPackage.Workbook.Worksheets.Add(uniqueName);
            }
            else
            {
                return excelPackage.Workbook.Worksheets.Add(SheetName);
            }
        }
    }
}
