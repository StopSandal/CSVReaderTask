using CSVReaderTask.Helpers.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace CSVReaderTask.Models.ViewModels
{
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

        public ObservableCollection<Person> People { get; set; }
        public ICollectionView PeopleView { get; set; }

        public FilterVM(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            People = new ObservableCollection<Person>(LoadData().Result);
            PeopleView = CollectionViewSource.GetDefaultView(People);
            PeopleView.Filter = FilterPeople;
        }
        private async Task<IEnumerable<Person>> LoadData() 
        {
            return await _unitOfWork.PersonRepository.GetAsync(null, x => x.OrderByDescending(x=>x.Date));
        }
        private void ApplyFilters()
        {
            PeopleView.Refresh();
        }
        public async void RefreshData()
        {
            var people = await LoadData();
            People = new ObservableCollection<Person>(people);
            ApplyFilters();
        }
        private bool FilterPeople(object item)
        {
            if (item is Person person)
            {
                return (DateFrom == null || person.Date >= DateFrom) &&
                       (DateTo == null || person.Date <= DateTo) &&
                       (string.IsNullOrEmpty(FirstName) || person.FirstName.IndexOf(FirstName, StringComparison.OrdinalIgnoreCase) >= 0) &&
                       (string.IsNullOrEmpty(LastName) || person.LastName.IndexOf(LastName, StringComparison.OrdinalIgnoreCase) >= 0) &&
                       (string.IsNullOrEmpty(SurName) || person.SurName.IndexOf(SurName, StringComparison.OrdinalIgnoreCase) >= 0) &&
                       (string.IsNullOrEmpty(City) || person.City.IndexOf(City, StringComparison.OrdinalIgnoreCase) >= 0) &&
                       (string.IsNullOrEmpty(Country) || person.Country.IndexOf(Country, StringComparison.OrdinalIgnoreCase) >= 0);
            }
            return false;
        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
