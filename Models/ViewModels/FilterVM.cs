using CSVReaderTask.Helpers.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
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
        private string _country;
        private string _firstName;
        private string _lastName;
        private string _surName;
        private string _city;
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="FilterVM"/> class.
        /// </summary>
        /// <param name="unitOfWork">Unit of work for data operations.</param>
        public FilterVM(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            People = new ObservableCollection<Person>(LoadData().Result);
            PeopleView = CollectionViewSource.GetDefaultView(People);
            PeopleView.Filter = FilterPeople;
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
                ApplyFilters();
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
                ApplyFilters();
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
                ApplyFilters();
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
                ApplyFilters();
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
                ApplyFilters();
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
                ApplyFilters();
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
                ApplyFilters();
            }
        }

        /// <summary>
        /// Loads data asynchronously from the repository.
        /// </summary>
        private async Task<IEnumerable<Person>> LoadData()
        {
            return await _unitOfWork.PersonRepository.GetAsync(null, x => x.OrderByDescending(x => x.Date));
        }

        /// <summary>
        /// Applies filters to the PeopleView collection.
        /// </summary>
        private void ApplyFilters()
        {
            PeopleView.Refresh();
        }

        /// <summary>
        /// Refreshes the data by reloading from the repository.
        /// </summary>
        public async void RefreshData()
        {
            var people = await LoadData();
            Application.Current.Dispatcher.Invoke(() =>
            {
                People.Clear();
                foreach (var person in people)
                {
                    People.Add(person);
                }
                ((ICollectionView)PeopleView).Refresh();
            });
            ApplyFilters();
        }

        /// <summary>
        /// Filters the people collection based on set filter criteria.
        /// </summary>
        /// <param name="item">Object to filter (expected to be a Person).</param>
        /// <returns>True if the item matches the filter criteria, otherwise false.</returns>
        private bool FilterPeople(object item)
        {
            if (item is Person person)
            {
                return (DateFrom == null || person.Date >= DateFrom) &&
                       (DateTo == null || person.Date <= DateTo) &&
                       (string.IsNullOrEmpty(FirstName) || person.FirstName.StartsWith(FirstName, StringComparison.OrdinalIgnoreCase)) &&
                       (string.IsNullOrEmpty(LastName) || person.LastName.StartsWith(LastName, StringComparison.OrdinalIgnoreCase)) &&
                       (string.IsNullOrEmpty(SurName) || person.SurName.StartsWith(SurName, StringComparison.OrdinalIgnoreCase)) &&
                       (string.IsNullOrEmpty(City) || person.City.StartsWith(City, StringComparison.OrdinalIgnoreCase)) &&
                       (string.IsNullOrEmpty(Country) || person.Country.StartsWith(Country, StringComparison.OrdinalIgnoreCase));
            }
            return false;
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
