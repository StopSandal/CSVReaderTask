using CSVReaderTask.Helpers.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace CSVReaderTask.Models.ViewModels
{
    /// <summary>
    /// View model for filtering and displaying Person data in a WPF application.
    /// </summary>
    public class FilterVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private DateTime? _dateFrom;
        private DateTime? _dateTo;
        private string _country = "";
        private string _firstName = "";
        private string _lastName = "";
        private string _surName = "";
        private string _city = "";
        private readonly IUnitOfWork _unitOfWork;

        private const int PageSize = 1000;
        private const int FilterDelayMilliseconds = 2000;
        private Timer _filterTimer;
        private bool _isFilterPending;

        /// <summary>
        /// Initializes a new instance of the <see cref="FilterVM"/> class.
        /// </summary>
        /// <param name="unitOfWork">Unit of work for data operations.</param>
        public FilterVM(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            People = new ObservableCollection<Person>();
            PeopleView = CollectionViewSource.GetDefaultView(People);

            _filterTimer = new Timer(FilterTimerCallback, null, Timeout.Infinite, Timeout.Infinite);
        }

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
            return await _unitOfWork.PersonRepository.GetAsync(
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
        }

        /// <summary>
        /// Refreshes the data by reloading from the repository.
        /// </summary>
        public async Task RefreshDataAsync()
        {
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
        /// Raises the PropertyChanged event when a property value changes.
        /// </summary>
        /// <param name="name">Name of the property that changed.</param>
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
