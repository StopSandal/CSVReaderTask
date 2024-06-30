using CSVReaderTask.EF;
using CSVReaderTask.Helpers.Interfaces;
using CSVReaderTask.Models;
using CSVReaderTask.Models.ViewModels;
using Microsoft.Win32;
using Syncfusion.Data.Extensions;
using System.Text;
using System.Windows;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;


namespace CSVReaderTask
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IMainWindowService _mainWindowService;
        private readonly FilterVM _filterVM;

        public MainWindow(IMainWindowService mainWindowService, FilterVM filterVM)
        {
            _mainWindowService = mainWindowService;
            _filterVM = filterVM;
            DataContext = _filterVM;
            InitializeComponent();

        }

        private async void ReadCsvFileAndLoadToDB(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "CSV files (*.csv)|*.csv"
            };
            if(openFileDialog.ShowDialog() == true)
            {
                await _mainWindowService.ReadCSVFileAsync(openFileDialog.FileName);
                _filterVM.RefreshData();
            }
        }
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