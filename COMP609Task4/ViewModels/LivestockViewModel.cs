using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using COMP609Task4.Models;

namespace COMP609Task4.ViewModels
{
    // ViewModel for managing livestock data
    public class LivestockViewModel : INotifyPropertyChanged
    {
        // Database instance for accessing livestock data
        private readonly Database _database;

        // Observable collections for holding all livestock and filtered livestock data
        private ObservableCollection<Stock> _livestock;
        private ObservableCollection<Stock> _filteredLivestock;

        // Properties

        // Property to access the database instance
        public Database Database => _database; // Property to access the database instance

        // Collection holding all livestock data
        public ObservableCollection<Stock> Livestock
        {
            get => _livestock;
            set
            {
                _livestock = value;
                OnPropertyChanged(nameof(Livestock));
            }
        }

        // Collection holding filtered livestock data
        public ObservableCollection<Stock> FilteredLivestock
        {
            get => _filteredLivestock;
            set
            {
                _filteredLivestock = value;
                OnPropertyChanged(nameof(FilteredLivestock));
                CalculateTotals();
            }
        }

        // Constructor
        public LivestockViewModel()
        {
            _database = new Database();
            LoadData();
            FilteredLivestock = new ObservableCollection<Stock>(Livestock); // Initialize with all items
        }

        // Load livestock data from the database
        public void LoadData()
        {
            var livestockData = _database.ReadItems();
            Livestock = livestockData != null ? new ObservableCollection<Stock>(livestockData) : new ObservableCollection<Stock>();
            FilteredLivestock = new ObservableCollection<Stock>(Livestock);
        }

        // Filter livestock by type
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

        // Filter livestock by type and colour
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

        // Totals properties      
        private int _totalStockCount;
        private decimal _totalCost;
        private decimal _totalWeight;
        private string _totalColours;
        private decimal _totalMilk;
        private decimal _totalWool;

        // Properties for storing total count, cost, weight, colours, milk, and wool of filtered livestock
        public int TotalStockCount
        {
            get => _totalStockCount;
            set
            {
                _totalStockCount = value;
                OnPropertyChanged(nameof(TotalStockCount));
            }
        }

        public decimal TotalCost
        {
            get => _totalCost;
            set
            {
                _totalCost = value;
                OnPropertyChanged(nameof(TotalCost));
            }
        }

        public decimal TotalWeight
        {
            get => _totalWeight;
            set
            {
                _totalWeight = value;
                OnPropertyChanged(nameof(TotalWeight));
            }
        }

        public string TotalColours
        {
            get => _totalColours;
            set
            {
                _totalColours = value;
                OnPropertyChanged(nameof(TotalColours));
            }
        }

        public decimal TotalMilk
        {
            get => _totalMilk;
            set
            {
                _totalMilk = value;
                OnPropertyChanged(nameof(TotalMilk));
            }
        }

        public decimal TotalWool
        {
            get => _totalWool;
            set
            {
                _totalWool = value;
                OnPropertyChanged(nameof(TotalWool));
            }
        }

        // Avg properties
        private string _avgStockDisplay;
        private decimal _avgCostDisplay;
        private decimal _avgWeightDisplay;
        private string _avgColoursDisplay;
        private decimal _avgMilkDisplay;
        private decimal _avgWoolDisplay;

        // Properties for storing avg count, cost, weight, colours, milk, and wool of filtered livestock
        public string AvgStockDisplay
        {
            get => _avgStockDisplay;
            set
            {
                _avgStockDisplay = value;
                OnPropertyChanged(nameof(AvgStockDisplay));
            }
        }
      
        public decimal AvgCostDisplay
        {
            get => _avgCostDisplay;
            set
            {
                _avgCostDisplay = value;
                OnPropertyChanged(nameof(AvgCostDisplay));
            }
        }
       
        public decimal AvgWeightDisplay
        {
            get => _avgWeightDisplay;
            set
            {
                _avgWeightDisplay = value;
                OnPropertyChanged(nameof(AvgWeightDisplay));
            }
        }
       
        public string AvgColoursDisplay
        {
            get => _avgColoursDisplay;
            set
            {
                _avgColoursDisplay = value;
                OnPropertyChanged(nameof(AvgColoursDisplay));
            }
        }
       
        public decimal AvgMilkDisplay
        {
            get => _avgMilkDisplay;
            set
            {
                _avgMilkDisplay = value;
                OnPropertyChanged(nameof(AvgMilkDisplay));
            }
        }
       
        public decimal AvgWoolDisplay
        {
            get => _avgWoolDisplay;
            set
            {
                _avgWoolDisplay = value;
                OnPropertyChanged(nameof(AvgWoolDisplay));
            }
        }

        // Calculate totals and averages of filtered livestock data
        private void CalculateTotals()
        {
            int totalStockCount = Livestock.Count;
            // Calculate total count, cost, weight, and colours of filtered livestock
            TotalStockCount = FilteredLivestock.Count;
            TotalCost = FilteredLivestock.Sum(s => s.Cost);
            TotalWeight = FilteredLivestock.Sum(s => s.Weight);

            var colourGroups = FilteredLivestock.GroupBy(s => s.Colour).ToDictionary(g => g.Key, g => g.Count());
            TotalColours = $"R: {colourGroups.GetValueOrDefault("Red", 0)} B: {colourGroups.GetValueOrDefault("Black", 0)} W: {colourGroups.GetValueOrDefault("White", 0)}";
            // Calculate total milk and total wool of filtered livestock
            TotalMilk = FilteredLivestock
                .OfType<Cow>()
                .Sum(cow => cow.Milk.GetValueOrDefault());

            TotalWool = FilteredLivestock
                .OfType<Sheep>()
                .Sum(sheep => sheep.Wool.GetValueOrDefault());


            // Calculate average stock percentage and convert to string
            double avgStockPercentage = TotalStockCount > 0 ? ((double)FilteredLivestock.Count / totalStockCount * 100) : 0;
            AvgStockDisplay = $"{avgStockPercentage:F0}%";


            // Calculate average cost
            AvgCostDisplay = FilteredLivestock.Count > 0 ? TotalCost / FilteredLivestock.Count : 0;

            // Calculate average weight
            AvgWeightDisplay = FilteredLivestock.Count > 0 ? TotalWeight / FilteredLivestock.Count : 0;

            // Calculate average milk
            double totalMilkCount = FilteredLivestock
                .OfType<Cow>()
                .Where(cow => cow.Milk.HasValue && cow.Milk != 0) // Filter out items with null or 0 value for Milk
                .Sum(cow => cow.Milk.GetValueOrDefault());

            int milkItemCount = FilteredLivestock
                .OfType<Cow>()
                .Count(cow => cow.Milk.HasValue && cow.Milk != 0);

            AvgMilkDisplay = milkItemCount > 0 ? (decimal)(totalMilkCount / milkItemCount) : 0;


            // Calculate average wool
            double totalWoolCount = FilteredLivestock
                .OfType<Sheep>()
                .Where(sheep => sheep.Wool.HasValue && sheep.Wool != 0) // Filter out items with '-' value for Wool
                .Sum(sheep => sheep.Wool.GetValueOrDefault());

            int woolItemCount = FilteredLivestock
                .OfType<Sheep>()
                .Count(sheep => sheep.Wool.HasValue && sheep.Wool != 0);

            AvgWoolDisplay = woolItemCount > 0 ? (decimal)totalWoolCount / woolItemCount : 0;

            // Format totals and averages
            if (FilteredLivestock.Count > 0)
            {
                TotalCost = Math.Round(TotalCost, 2);
                TotalWeight = Math.Round(TotalWeight, 2);
                AvgCostDisplay = Math.Round(AvgCostDisplay, 2);
                AvgWeightDisplay = Math.Round(AvgWeightDisplay, 2);
            }
            else
            {
                TotalMilk = Math.Round(TotalMilk, 0);
                TotalWool = Math.Round(TotalWool, 0);
                AvgMilkDisplay = Math.Round(AvgMilkDisplay, 0);
                AvgWoolDisplay = Math.Round(AvgWoolDisplay, 0);
            }

            // Calculate percentage of each color for the averages line
            double redPercentage = totalStockCount > 0 ? (double)colourGroups.GetValueOrDefault("Red", 0) / totalStockCount * 100 : 0;
            double blackPercentage = totalStockCount > 0 ? (double)colourGroups.GetValueOrDefault("Black", 0) / totalStockCount * 100 : 0;
            double whitePercentage = totalStockCount > 0 ? (double)colourGroups.GetValueOrDefault("White", 0) / totalStockCount * 100 : 0;

            // Concatenate total colours with percentages for the averages line
            AvgColoursDisplay = $"{redPercentage:F0}% {blackPercentage:F0}% {whitePercentage:F0}%";

            // Update OnPropertyChanged for totals
            OnPropertyChanged(nameof(TotalStockCount));
            OnPropertyChanged(nameof(TotalCost));
            OnPropertyChanged(nameof(TotalWeight));
            OnPropertyChanged(nameof(TotalColours));
            OnPropertyChanged(nameof(TotalMilk));
            OnPropertyChanged(nameof(TotalWool));

            // Update OnPropertyChanged for averages
            OnPropertyChanged(nameof(AvgStockDisplay));
            OnPropertyChanged(nameof(AvgCostDisplay));
            OnPropertyChanged(nameof(AvgWeightDisplay));
            OnPropertyChanged(nameof(AvgMilkDisplay));
            OnPropertyChanged(nameof(AvgWoolDisplay));
            OnPropertyChanged(nameof(AvgColoursDisplay));
        }

        // INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}