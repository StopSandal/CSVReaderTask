using CSVReaderTask.Commands;
using CSVReaderTask.Helpers.Interfaces;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace CSVReaderTask.Models.ViewModels
{
    /// <summary>
    /// View model for filtering and displaying Person data in a WPF application.
    /// </summary>
    public class FilterVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private readonly Semaphore semaphore = new (1, 1);

        private DateTime? _dateFrom;
        private DateTime? _dateTo;
        private string _country = "";
        private string _firstName = "";
        private string _lastName = "";
        private string _surName = "";
        private string _city = "";
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMainWindowService _mainWindowService;
        private readonly IFileDialog _fileDialog;
        private readonly IMessageDialog _messageDialog;

        private const int PageSize = 20000;
        private const int FilterDelayMilliseconds = 1000;

        private const string CsvOpenFilter = "CSV files (*.csv)|*.csv";

        private const string XmlSaveFilter = "XML files (*.xml)|*.xml|Excel files (*.xlsx)|*.xlsx";
        private const string XmlSaveExtension = ".xml";

        private const string ExcelSaveFilter = "Excel files (*.xlsx)|*.xlsx";
        private const string ExcelSaveExtension = ".xlsx";

        private const string SaveTitle = "Save File As";


        private readonly Timer _filterTimer;
        private bool _isFilterPending;

        /// <summary>
        /// Initializes a new instance of the <see cref="FilterVM"/> class.
        /// </summary>
        /// <param name="unitOfWork">Unit of work for data operations.</param>
        /// <param name="mainWindowService">Service for main window operations.</param>
        /// <param name="fileDialog">Service for file dialog operations.</param>
        /// <param name="messageDialog">Service for displaying messages.</param>
        public FilterVM(IUnitOfWork unitOfWork, IMainWindowService mainWindowService, IFileDialog fileDialog, IMessageDialog messageDialog)
        {
            People = new ObservableCollection<Person>();
            PeopleView = CollectionViewSource.GetDefaultView(People);
            _filterTimer = new Timer(FilterTimerCallback, null, Timeout.Infinite, Timeout.Infinite);

            _unitOfWork = unitOfWork;
            _mainWindowService = mainWindowService;
            _fileDialog = fileDialog;
            _messageDialog = messageDialog;

            ReadCsvFileCommand = new RelayCommand(async _ => await ReadCsvFileAsync());
            ExportToExcelCommand = new RelayCommand(async _ => await ExportToExcelFileAsync());
            ExportToXmlCommand = new RelayCommand(async _ => await ExportToXMLFileAsync());
            WindowLoadedCommand = new RelayCommand(async _ => await RefreshDataAsync());
        }

        public ICommand ReadCsvFileCommand { get; }
        public ICommand ExportToExcelCommand { get; }
        public ICommand ExportToXmlCommand { get; }
        public ICommand WindowLoadedCommand { get; }
        
        /// <summary>
        /// Gets or sets the collection of people.
        /// </summary>
        public ObservableCollection<Person> People { get; set; }

        /// <summary>
        /// Gets or sets the filtered view of people collection.
        /// </summary>
        public ICollectionView PeopleView { get; set; }

        /// <summary>
        /// Gets or sets the starting date filter.
        /// </summary>
        public DateTime? DateFrom
        {
            get => _dateFrom;
            set
            {
                _dateFrom = value;
                OnPropertyChanged(nameof(DateFrom));
                ScheduleFilterApplication();
            }
        }

        /// <summary>
        /// Gets or sets the ending date filter.
        /// </summary>
        public DateTime? DateTo
        {
            get => _dateTo;
            set
            {
                _dateTo = value;
                OnPropertyChanged(nameof(DateTo));
                ScheduleFilterApplication();
            }
        }

        /// <summary>
        /// Gets or sets the first name filter.
        /// </summary>
        public string FirstName
        {
            get => _firstName;
            set
            {
                _firstName = value;
                OnPropertyChanged(nameof(FirstName));
                ScheduleFilterApplication();
            }
        }

        /// <summary>
        /// Gets or sets the last name filter.
        /// </summary>
        public string LastName
        {
            get => _lastName;
            set
            {
                _lastName = value;
                OnPropertyChanged(nameof(LastName));
                ScheduleFilterApplication();
            }
        }

        /// <summary>
        /// Gets or sets the surname filter.
        /// </summary>
        public string SurName
        {
            get => _surName;
            set
            {
                _surName = value;
                OnPropertyChanged(nameof(SurName));
                ScheduleFilterApplication();
            }
        }

        /// <summary>
        /// Gets or sets the city filter.
        /// </summary>
        public string City
        {
            get => _city;
            set
            {
                _city = value;
                OnPropertyChanged(nameof(City));
                ScheduleFilterApplication();
            }
        }

        /// <summary>
        /// Gets or sets the country filter.
        /// </summary>
        public string Country
        {
            get => _country;
            set
            {
                _country = value;
                OnPropertyChanged(nameof(Country));
                ScheduleFilterApplication();
            }
        }

        /// <summary>
        /// Loads filtered data asynchronously from the repository.
        /// </summary>
        private async Task<IEnumerable<Person>> LoadDataAsync()
        {

             var collection = await _unitOfWork.PersonRepository.GetAsync(
                filter: p =>
                    (DateFrom == null || p.Date >= DateFrom) &&
                    (DateTo == null || p.Date <= DateTo) &&
                    (string.IsNullOrEmpty(FirstName) || p.FirstName.StartsWith(FirstName)) &&
                    (string.IsNullOrEmpty(LastName) || p.LastName.StartsWith(LastName)) &&
                    (string.IsNullOrEmpty(SurName) || p.SurName.StartsWith(SurName)) &&
                    (string.IsNullOrEmpty(City) || p.City.StartsWith(City)) &&
                    (string.IsNullOrEmpty(Country) || p.Country.StartsWith(Country)),
                orderBy: x => x.OrderByDescending(x => x.Date),
                takeAmount: PageSize
            );

            return collection;

        }

        /// <summary>
        /// Refreshes the data by reloading from the repository.
        /// </summary>
        private async Task RefreshDataAsync()
        {

            semaphore.WaitOne();
            var people = await LoadDataAsync();

            Application.Current.Dispatcher.Invoke(() =>
            {
                People.Clear();
                foreach (var person in people)
                {
                    People.Add(person);
                }
                PeopleView.Refresh();
            });
            semaphore.Release();

        }
        /// <summary>
        /// Schedules the application of filters with a delay to avoid rapid querying.
        /// </summary>
        private void ScheduleFilterApplication()
        {
            if (!_isFilterPending)
            {
                _isFilterPending = true;
                _filterTimer.Change(FilterDelayMilliseconds, Timeout.Infinite);
            }
        }

        /// <summary>
        /// Callback method for the filter timer to apply the filters after the delay.
        /// </summary>
        private async void FilterTimerCallback(object state)
        {
            _isFilterPending = false;
            await RefreshDataAsync();
        }
        /// <summary>
        /// Asynchronously reads a CSV file and loads data into the application.
        /// </summary>
        private async Task ReadCsvFileAsync()
        {
            var filePath = _fileDialog.ShowOpenDialog(CsvOpenFilter);
            if (filePath != null)
            {
                try
                {
                    var handledCount = await _mainWindowService.ReadCSVFileAsync(filePath);
                    _messageDialog.ShowOK($"File was successfully read. Total added records {handledCount}", "Success");
                }
                catch (Exception ex)
                {
                    _messageDialog.ShowError($"File reading caused exception {ex.Message}");
                }

                await RefreshDataAsync();
            }
        }
        /// <summary>
        /// Asynchronously exports data to an Excel file.
        /// </summary>
        private async Task ExportToExcelFileAsync()
        {
            var filePath = _fileDialog.ShowSaveDialog(ExcelSaveFilter, ExcelSaveExtension, SaveTitle);
            if (filePath != null)
            {
                var filteredCollection = PeopleView.OfType<Person>();
                await _mainWindowService.SavePersonInfoToExcelAsync(filePath, filteredCollection);
                _messageDialog.ShowMessage("Data was successfully exported to Excel.", "Success");
            }
        }
        /// <summary>
        /// Asynchronously exports data to an XML file.
        /// </summary>
        private async Task ExportToXMLFileAsync()
        {
            var filePath = _fileDialog.ShowSaveDialog(XmlSaveFilter, XmlSaveExtension, SaveTitle);
            if (filePath != null)
            {
                var filteredCollection = PeopleView.OfType<Person>();
                await _mainWindowService.SavePersonInfoToXMLAsync(filePath, filteredCollection);
                _messageDialog.ShowOK("Data was successfully exported to XML.", "Success");
            }
        }

        /// <summary>
        /// Raises the PropertyChanged event when a property value changes.
        /// </summary>
        /// <param name="name">Name of the property that changed.</param>
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
