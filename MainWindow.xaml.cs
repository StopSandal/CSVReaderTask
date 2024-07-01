using CSVReaderTask.Helpers.Interfaces;
using CSVReaderTask.Models;
using CSVReaderTask.Models.ViewModels;
using Microsoft.Win32;
using System.Windows;


namespace CSVReaderTask
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IMainWindowService _mainWindowService;
        private readonly FilterVM _filterVM;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        /// <param name="mainWindowService">Service for main window operations.</param>
        /// <param name="filterVM">View model for filtering and displaying data.</param>
        public MainWindow(IMainWindowService mainWindowService, FilterVM filterVM)
        {
            _mainWindowService = mainWindowService;
            _filterVM = filterVM;
            DataContext = _filterVM;
            InitializeComponent();
        }
        
        /// <summary>
        /// Event handler for reading CSV file and loading data to the database.
        /// </summary>
        private async void ReadCsvFileAndLoadToDB(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "CSV files (*.csv)|*.csv"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    var handledCount = await _mainWindowService.ReadCSVFileAsync(openFileDialog.FileName);
                    MessageBox.Show($"File was successfully read. Total added records {handledCount}","Success",MessageBoxButton.OK,MessageBoxImage.Information);
                }
                catch(Exception ex)
                {
                    MessageBox.Show($"File reading caused exception {ex.Message}");
                }

                _filterVM.RefreshData();
            }
        }

        /// <summary>
        /// Event handler for exporting data to Excel file.
        /// </summary>
        private async void ExportToExcelFile(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Excel files (*.xlsx)|*.xlsx",
                DefaultExt = ".xlsx",
                Title = "Save Excel File As"
            };
            if (saveFileDialog.ShowDialog() == true)
            {
                var filteredCollection = _filterVM.PeopleView.OfType<Person>();
                await _mainWindowService.SavePersonInfoToExcelAsync(saveFileDialog.FileName, filteredCollection);
            }
        }

        /// <summary>
        /// Event handler for exporting data to XML file.
        /// </summary>
        private async void ExportToXMLFile(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "XML files (*.xml)|*.xml|Excel files (*.xlsx)|*.xlsx",
                DefaultExt = ".xml",
                Title = "Save File As",
            };
            if (saveFileDialog.ShowDialog() == true)
            {
                var filteredCollection = _filterVM.PeopleView.OfType<Person>();
                await _mainWindowService.SavePersonInfoToXMLAsync(saveFileDialog.FileName, filteredCollection);
            }
        }
    }
}