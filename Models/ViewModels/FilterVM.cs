﻿using CSVReaderTask.Helpers.Interfaces;
using MahApps.Metro.Controls.Dialogs;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq.Expressions;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using RelayCommand = CSVReaderTask.Commands.RelayCommand;

namespace CSVReaderTask.Models.ViewModels
{
    /// <summary>
    /// View model for filtering and displaying Person data in a WPF application.
    /// </summary>
    public class FilterVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private DateTime? _dateFrom;
        private DateTime? _dateTo;
        private string _country = "";
        private string _firstName = "";
        private string _lastName = "";
        private string _surName = "";
        private string _city = "";
        private string _currentLanguage = CultureInfo.CurrentCulture.ToString();

        private readonly ILocalizationService _localizationService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMainWindowService _mainWindowService;
        private readonly IFileDialog _fileDialog;
        private readonly IMessageDialog _messageDialog;
        private readonly IProgressDialogService _progressDialog;
        private readonly IInitializeOnStartService _initializeOnStartService;

        private const int PageSize = 500;
        private const int FilterDelayMilliseconds = 1500;

        private const string CsvOpenFilter = "CSV files (*.csv)|*.csv";

        private const string XmlSaveFilter = "XML files (*.xml)|*.xml|Excel files (*.xlsx)|*.xlsx";
        private const string XmlSaveExtension = ".xml";

        private const string ExcelSaveFilter = "Excel files (*.xlsx)|*.xlsx";
        private const string ExcelSaveExtension = ".xlsx";

        private const string SaveTitle = "Save File As";


        private readonly Timer _filterTimer;
        private bool _isFilterPending;
        private int _currentPage = 1;
        private int _totalPages;

        private readonly Expression<Func<Person, bool>> personFilter;

        private bool _shouldPageChangeUpdateData = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="FilterVM"/> class.
        /// </summary>
        /// <param name="unitOfWork">Unit of work for data operations.</param>
        /// <param name="mainWindowService">Service for main window operations.</param>
        /// <param name="fileDialog">Service for file dialog operations.</param>
        /// <param name="messageDialog">Service for displaying messages.</param>
        public FilterVM(IUnitOfWork unitOfWork
            , IMainWindowService mainWindowService
            , IFileDialog fileDialog
            , IMessageDialog messageDialog
            , IInitializeOnStartService initializeOnStartService
            , IProgressDialogService progressDialog
            , ILocalizationService localizationService)
        {
            People = new ObservableCollection<Person>();
            PeopleView = CollectionViewSource.GetDefaultView(People);
            _filterTimer = new Timer(FilterTimerCallback, null, Timeout.Infinite, Timeout.Infinite);

            _unitOfWork = unitOfWork;
            _mainWindowService = mainWindowService;
            _fileDialog = fileDialog;
            _messageDialog = messageDialog;
            _localizationService = localizationService;
            _progressDialog = progressDialog;
            _initializeOnStartService = initializeOnStartService;

            ReadCsvFileCommand = new RelayCommand(async _ => await ReadCsvFileAsync());
            ExportToExcelCommand = new RelayCommand(async _ => await ExportToExcelFileAsync());
            ExportToXmlCommand = new RelayCommand(async _ => await ExportToXMLFileAsync());
            WindowLoadedCommand = new RelayCommand(async _ => await InitDBandData());

            NextPageCommand = new RelayCommand(_ => CurrentPage++, _ => CurrentPage < TotalPages);
            PreviousPageCommand = new RelayCommand(_ => CurrentPage--, _ => CurrentPage > 1);

            personFilter = p => // Setting filter
                   (DateFrom == null || p.Date >= DateFrom) &&
                   (DateTo == null || p.Date <= DateTo) &&
                   (string.IsNullOrEmpty(FirstName) || p.FirstName.StartsWith(FirstName)) &&
                   (string.IsNullOrEmpty(LastName) || p.LastName.StartsWith(LastName)) &&
                   (string.IsNullOrEmpty(SurName) || p.SurName.StartsWith(SurName)) &&
                   (string.IsNullOrEmpty(City) || p.City.StartsWith(City)) &&
                   (string.IsNullOrEmpty(Country) || p.Country.StartsWith(Country));
        }

        public string FirstNameColumnHeader => _localizationService.GetString("FirstNameColumn");
        public string LastNameColumnHeader => _localizationService.GetString("LastNameColumn");
        public string SurNameColumnHeader => _localizationService.GetString("SurNameColumn");
        public string CityColumnHeader => _localizationService.GetString("CityColumn");
        public string CountryColumnHeader => _localizationService.GetString("CountryColumn");
        public string DateColumnHeader => _localizationService.GetString("DateColumn");

        public ICommand ReadCsvFileCommand { get; }
        public ICommand ExportToExcelCommand { get; }
        public ICommand ExportToXmlCommand { get; }
        public ICommand WindowLoadedCommand { get; }
        public ICommand NextPageCommand { get; }
        public ICommand PreviousPageCommand { get; }

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
        /// Gets or sets the app localization.
        /// </summary>
        public string CurrentLanguage
        {
            get => _currentLanguage;
            set
            {
                if (_currentLanguage != value)
                {
                    _currentLanguage = value;
                    OnPropertyChanged(nameof(CurrentLanguage));
                    _localizationService.ChangeCulture(_currentLanguage);
                    OnPropertyChanged(null);
                }
            }
        }
        /// <summary>
        /// Gets or sets the current datagrid page.
        /// </summary>
        public int CurrentPage
        {
            get => _currentPage;
            set
            {
                if (value != _currentPage)
                {
                    _currentPage = value;
                    OnPropertyChanged(nameof(CurrentPage));

                    if (_shouldPageChangeUpdateData)
                        ScheduleFilterApplication();
                }
            }
        }
        /// <summary>
        /// Gets or sets the total count of the datagrid page.
        /// </summary>
        public int TotalPages
        {
            get => _totalPages;
            set
            {
                _totalPages = value;
                OnPropertyChanged(nameof(TotalPages));
            }
        }

        /// <summary>
        /// Returns localized string
        /// </summary>
        public string this[string key] => _localizationService.GetString(key);

        /// <summary>
        /// Loads filtered data asynchronously from the repository.
        /// </summary>
        private async Task<IEnumerable<Person>> LoadDataAsync()
        {
            var collection = await _unitOfWork.PersonRepository.GetAsync(
             filter: personFilter,
             orderBy: x => x.OrderByDescending(x => x.Date),
             takeAmount: PageSize,
             skipAmount: (CurrentPage - 1) * PageSize
             );

            var totalCount = await _unitOfWork.PersonRepository.CountAsync(filter: personFilter);

            var newPages = (int)Math.Ceiling((double)totalCount / PageSize);

            if (newPages != TotalPages)
            {
                TotalPages = newPages;

                _shouldPageChangeUpdateData = false;
                CurrentPage = 1;
                _shouldPageChangeUpdateData = true;
            }

            return collection;

        }

        /// <summary>
        /// Refreshes the data by reloading from the repository.
        /// </summary>
        private async Task RefreshDataAsync()
        {
            var dialog = await _progressDialog.ShowProgressAsync(this,
                _localizationService.GetString("FetchingData"),
                _localizationService.GetString("PleaseWaitFetchingData"));
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
            await _progressDialog.HideProgressAsync(dialog);
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

            await RefreshDataAsync();
            _isFilterPending = false;
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
                    var dialog = await _progressDialog.ShowProgressAsync(this,
                        _localizationService.GetString("ReadingYourFile"),
                        _localizationService.GetString("PleaseWaitUntilFileRead"));

                    var handledCount = await _mainWindowService.ReadCSVFileAsync(filePath);

                    await _progressDialog.HideProgressAsync(dialog);

                    _messageDialog.ShowOK(
                        string.Format(_localizationService.GetString("FileSuccessfullyRead"), handledCount),
                            _localizationService.GetString("SuccessDialogTitle"));
                }
                catch (Exception ex)
                {
                    _messageDialog.ShowError(string.Format(_localizationService.GetString("FileReadingException"), ex.Message));
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
                var dialog = await _progressDialog.ShowProgressAsync(this,
                    _localizationService.GetString("ExportingToExcel"),
                    _localizationService.GetString("PleaseWaitExportingToExcel"));

                var filteredData = _unitOfWork.PersonRepository.GetAsyncEnumerable(personFilter);

                await _mainWindowService.SavePersonInfoToExcelAsync(filePath, filteredData);

                await _progressDialog.HideProgressAsync(dialog);

                _messageDialog.ShowMessage(
                    _localizationService.GetString("DataSuccessfullyExportedToExcel"),
                    _localizationService.GetString("SuccessDialogTitle"));
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
                var dialog = await _progressDialog.ShowProgressAsync(this,
                    _localizationService.GetString("ExportingToXML"),
                    _localizationService.GetString("PleaseWaitExportingToXML"));

                var filteredData = _unitOfWork.PersonRepository.GetAsyncEnumerable(personFilter);

                await _mainWindowService.SavePersonInfoToXMLAsync(filePath, filteredData);

                await _progressDialog.HideProgressAsync(dialog);

                _messageDialog.ShowOK(
                    _localizationService.GetString("DataSuccessfullyExportedToXML"),
                    _localizationService.GetString("SuccessDialogTitle"));
            }
        }
        /// <summary>
        /// Asynchronously get DB connection, handle if can't connect, otherwise loads initial data
        /// </summary>
        private async Task InitDBandData()
        {
            var progressDialog = await _progressDialog.ShowProgressAsync(
                this,
                _localizationService.GetString("ConnectionToDBTitle"),
                _localizationService.GetString("ConnectionToDBMessage"));

            bool success = false;

            // Run the initialization in a separate task
            await Task.Run(() => success = _initializeOnStartService.Initialize());

            await _progressDialog.HideProgressAsync(progressDialog);

            if (success)
            {
                await RefreshDataAsync();
            }
            else
            {
                bool retry;
                do
                {
                    var retryDialog = await _messageDialog.ShowRetryDialog(
                        this,
                        _localizationService.GetString("RetryConnectionPrompt")
                        );

                    retry = retryDialog == MessageDialogResult.Affirmative;

                    if (retry)
                    {
                        var connectionString = await _messageDialog.ShowInputDialog(
                            this,
                            _localizationService.GetString("EnterNewConnectionTitle"),
                            _localizationService.GetString("EnterNewConnectionPrompt"));

                        _initializeOnStartService.SetNewConnectionString(connectionString);

                        progressDialog = await _progressDialog.ShowProgressAsync(
                            this,
                            _localizationService.GetString("ConnectionToDBTitle"),
                            _localizationService.GetString("ConnectionToDBMessage"));

                        await Task.Run(() => success = _initializeOnStartService.Initialize());

                        await _progressDialog.HideProgressAsync(progressDialog);

                        if (success)
                        {
                            await RefreshDataAsync();
                        }
                    }
                    else
                    {
                        App.Current.Shutdown();
                    }
                } while (!success && retry);
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
