using CSVReaderTask.EF;
using CSVReaderTask.Helpers.Interfaces;
using Microsoft.Win32;
using System.Text;
using System.Windows;


namespace CSVReaderTask
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ICSVReader _csvReader;
        private readonly CSVContext _dbContext;

        public MainWindow(ICSVReader csvReader, CSVContext dbContext)
        {
            _csvReader = csvReader;
            _dbContext = dbContext;

            InitializeComponent();

        }

        private void ReadCsvFileAndLoadToDB(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*"
            };
            if(openFileDialog.ShowDialog() == true)
            {
                _csvReader.ReadFileAndSaveToDBAsync(openFileDialog.FileName);
            }
        }
    }
}