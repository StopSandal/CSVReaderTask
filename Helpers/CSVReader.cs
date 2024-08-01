using CSVReaderTask.Helpers.Interfaces;
using CSVReaderTask.Models;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace CSVReaderTask.Helpers
{
    /// <summary>
    /// Provides functionality to read a CSV file and save its contents to a database using Entity Framework Core.
    /// Implements <see cref="ICSVReader"/>.
    /// </summary>
    public class CSVReader : ICSVReader
    {
        private const int BunchReturnCount = 4096;
        private const char CSVDelimiter = ';';
        const int expectedParts = 6;
        string[] parts = new string[expectedParts];

        /// <summary>
        /// Initializes a new instance of the <see cref="CSVReader"/> class.
        /// </summary>
        public CSVReader()
        {
        }

        /// <inheritdoc />
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="filePath"/> is null or empty.</exception>
        /// <exception cref="FileNotFoundException">Thrown when the specified <paramref name="filePath"/> does not exist.</exception>
        /// <exception cref="IOException">Thrown when an error occurs during file reading.</exception>
        public async IAsyncEnumerable<IEnumerable<Person>> ReadFilePersonAsync(string filePath)
        {
            Debug.WriteLine("Start reading");
            var startTime = DateTime.Now;

            var personList = new List<Person>();

            using var streamReader = new StreamReader(filePath, Encoding.UTF8);
            while (!streamReader.EndOfStream)
            {
                var line = await streamReader.ReadLineAsync();
                if (!line.IsNullOrEmpty())
                {
                    var person = ParseLine(line);
                    personList.Add(person);

                    if (personList.Count >= BunchReturnCount)
                    {
                        yield return personList;
                        personList = new List<Person>();
                    }
                }
            }
            if (personList.Count > 0)
            {
                yield return personList;
            }
            Debug.WriteLine($"Time elapsed {DateTime.Now - startTime}");
        }

        private Person ParseLine(string line)
        {
            var span = line.AsSpan();

            int start = 0;
            int partIndex = 0;

            while (partIndex < expectedParts)
            {
                int delimiterIndex = span.Slice(start).IndexOf(CSVDelimiter);

                if (delimiterIndex == -1)
                {
                    delimiterIndex = span.Length - start;
                }

                parts[partIndex++] = span.Slice(start, delimiterIndex).ToString();

                start += delimiterIndex + 1;

                if (start >= span.Length && partIndex < expectedParts)
                {
                    throw new ArgumentException("The line does not contain enough data to create a Person object.");
                }
            }

            return new Person
            {
                Date = DateTime.Parse(parts[0]),
                FirstName = parts[1],
                LastName = parts[2],
                SurName = parts[3],
                City = parts[4],
                Country = parts[5]
            };
        }
    }
}
