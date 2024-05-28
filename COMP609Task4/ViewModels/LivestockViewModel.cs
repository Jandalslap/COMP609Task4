using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using COMP609Task4.Models;

namespace COMP609Task4.ViewModels
{
    public class LivestockViewModel : INotifyPropertyChanged
    {
        private readonly Database _database;
        private ObservableCollection<Stock> _livestock;
        private ObservableCollection<Stock> _filteredLivestock;

        public ObservableCollection<Stock> Livestock
        {
            get => _livestock;
            set
            {
                _livestock = value;
                OnPropertyChanged(nameof(Livestock));
            }
        }

        public ObservableCollection<Stock> FilteredLivestock
        {
            get => _filteredLivestock;
            set
            {
                _filteredLivestock = value;
                OnPropertyChanged(nameof(FilteredLivestock));
            }
        }

        public LivestockViewModel()
        {
            _database = new Database();
            LoadData();
            FilteredLivestock = new ObservableCollection<Stock>(Livestock); // Initialize with all items
        }

        private void LoadData()
        {
            var livestockData = _database.ReadItems();
            Livestock = livestockData != null ? new ObservableCollection<Stock>(livestockData) : new ObservableCollection<Stock>();
        }

        public void FilterStock(string selectedType)
        {
            if (selectedType == null)
            {
                FilteredLivestock = new ObservableCollection<Stock>(Livestock);
            }
            else
            {
                FilteredLivestock = new ObservableCollection<Stock>(Livestock.Where(item => item.Type == selectedType));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void FilterStockById(string id)
        {
            FilteredLivestock = new ObservableCollection<Stock>(Livestock.Where(item => item.Id.ToString() == id));
        }

        public void FilterStock(string selectedType, string selectedColour)
        {
            var allItems = _database.ReadItems();

            var filteredItems = allItems.AsEnumerable();

            if (!string.IsNullOrEmpty(selectedType))
            {
                filteredItems = filteredItems.Where(stock => stock.Type == selectedType);
            }

            if (!string.IsNullOrEmpty(selectedColour))
            {
                filteredItems = filteredItems.Where(stock => stock.Colour == selectedColour);
            }

            FilteredLivestock = new ObservableCollection<Stock>(filteredItems);
        }
    }
}
