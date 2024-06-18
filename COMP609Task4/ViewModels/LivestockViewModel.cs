using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using COMP609Task4.Models;

namespace COMP609Task4.ViewModels
{
    // ViewModel for managing livestock data and operations
    public class LivestockViewModel : INotifyPropertyChanged
    {
        #region Private Members
        // Database instance for accessing livestock data
        private readonly Database _database;

        // Observable collections for holding all livestock and filtered livestock data
        private ObservableCollection<Stock> _livestock;
        private ObservableCollection<Stock> _filteredLivestock;
        #endregion
        #region Properties
        // Property to access the database instance
        public Database Database => _database;

        // Collection holding all livestock data
        public ObservableCollection<Stock> Livestock
        {
            get => _livestock;
            set
            {
                _livestock = value;
                OnPropertyChanged(nameof(Livestock)); // Notify that the Livestock property has changed
            }
        }

        // Collection holding filtered livestock data based on criteria (e.g., type and color)
        public ObservableCollection<Stock> FilteredLivestock
        {
            get => _filteredLivestock;
            set
            {
                _filteredLivestock = value;
                OnPropertyChanged(nameof(FilteredLivestock)); // Notify that the FilteredLivestock property has changed
                CalculateTotals(); // Recalculate totals when the filtered collection changes
            }
        }
        #endregion
        #region Constructor
        // Initializes a new instance of the LivestockViewModel class
        public LivestockViewModel()
        {
            _database = new Database(); // Initialize the database instance
            LoadData(); // Load livestock data from the database
            FilteredLivestock = new ObservableCollection<Stock>(Livestock); // Initialize filtered collection with all items
        }
        #endregion
        #region Methods
        /// <summary>
        /// Loads all livestock data from the database.
        /// </summary>
        public void LoadData()
        {
            // Read all livestock data from the database
            var livestockData = _database.ReadItems();
            Livestock = livestockData != null ? new ObservableCollection<Stock>(livestockData) : new ObservableCollection<Stock>();

            // Initialize the filtered livestock collection with all items
            FilteredLivestock = new ObservableCollection<Stock>(Livestock);
        }

        /// <summary>
        /// Filters the livestock collection by type and colour.
        /// </summary>
        /// <param name="selectedType">The type of stock to filter by (e.g., Cow, Sheep).</param>
        /// <param name="selectedColour">The colour of stock to filter by.</param>
        public void FilterStock(string selectedType, string selectedColour)
        {
            // Get all items from the database
            var allItems = _database.ReadItems();
            var filteredItems = allItems.AsEnumerable();

            // Apply type filter if specified
            if (!string.IsNullOrEmpty(selectedType))
            {
                filteredItems = filteredItems.Where(stock => stock.Type == selectedType);
            }

            // Apply colour filter if specified
            if (!string.IsNullOrEmpty(selectedColour))
            {
                filteredItems = filteredItems.Where(stock => stock.Colour == selectedColour);
            }

            // Update the filtered livestock collection with the filtered items
            FilteredLivestock = new ObservableCollection<Stock>(filteredItems);
        }
        #endregion
        #region Totals and Averages
        // Fields for storing totals and averages
        private int _totalStockCount;
        private decimal _totalCost;
        private decimal _totalWeight;
        private string _totalColours;
        private decimal _totalMilk;
        private decimal _totalWool;

        // Fields for storing average values
        private string _avgStockDisplay;
        private decimal _avgCostDisplay;
        private decimal _avgWeightDisplay;
        private string _avgColoursDisplay;
        private decimal _avgMilkDisplay;
        private decimal _avgWoolDisplay;

        // Properties for storing totals

        // Total count of filtered livestock
        public int TotalStockCount
        {
            get => _totalStockCount;
            set
            {
                _totalStockCount = value;
                OnPropertyChanged(nameof(TotalStockCount));
            }
        }

        // Total cost of filtered livestock
        public decimal TotalCost
        {
            get => _totalCost;
            set
            {
                _totalCost = value;
                OnPropertyChanged(nameof(TotalCost));
            }
        }

        // Total weight of filtered livestock
        public decimal TotalWeight
        {
            get => _totalWeight;
            set
            {
                _totalWeight = value;
                OnPropertyChanged(nameof(TotalWeight));
            }
        }

        // Total colours of filtered livestock, represented in a summary string
        public string TotalColours
        {
            get => _totalColours;
            set
            {
                _totalColours = value;
                OnPropertyChanged(nameof(TotalColours));
            }
        }

        // Total milk produced by filtered livestock (Cows only)
        public decimal TotalMilk
        {
            get => _totalMilk;
            set
            {
                _totalMilk = value;
                OnPropertyChanged(nameof(TotalMilk));
            }
        }

        // Total wool produced by filtered livestock (Sheep only)
        public decimal TotalWool
        {
            get => _totalWool;
            set
            {
                _totalWool = value;
                OnPropertyChanged(nameof(TotalWool));
            }
        }

        // Properties for storing averages

        // Average stock percentage display
        public string AvgStockDisplay
        {
            get => _avgStockDisplay;
            set
            {
                _avgStockDisplay = value;
                OnPropertyChanged(nameof(AvgStockDisplay));
            }
        }

        // Average cost of filtered livestock
        public decimal AvgCostDisplay
        {
            get => _avgCostDisplay;
            set
            {
                _avgCostDisplay = value;
                OnPropertyChanged(nameof(AvgCostDisplay));
            }
        }

        // Average weight of filtered livestock
        public decimal AvgWeightDisplay
        {
            get => _avgWeightDisplay;
            set
            {
                _avgWeightDisplay = value;
                OnPropertyChanged(nameof(AvgWeightDisplay));
            }
        }

        // Average colours of filtered livestock, represented in a summary string
        public string AvgColoursDisplay
        {
            get => _avgColoursDisplay;
            set
            {
                _avgColoursDisplay = value;
                OnPropertyChanged(nameof(AvgColoursDisplay));
            }
        }

        // Average milk produced by filtered livestock (Cows only)
        public decimal AvgMilkDisplay
        {
            get => _avgMilkDisplay;
            set
            {
                _avgMilkDisplay = value;
                OnPropertyChanged(nameof(AvgMilkDisplay));
            }
        }

        // Average wool produced by filtered livestock (Sheep only)
        public decimal AvgWoolDisplay
        {
            get => _avgWoolDisplay;
            set
            {
                _avgWoolDisplay = value;
                OnPropertyChanged(nameof(AvgWoolDisplay));
            }
        }

        /// <summary>
        /// Calculates and updates the totals and averages of the filtered livestock data.
        /// </summary>
        private void CalculateTotals()
        {
            // Calculate total stock count, cost, and weight of filtered livestock
            TotalStockCount = FilteredLivestock.Count;
            TotalCost = FilteredLivestock.Sum(s => s.Cost);
            TotalWeight = FilteredLivestock.Sum(s => s.Weight);

            // Group filtered livestock by colour and count the number of each colour
            var colourGroups = FilteredLivestock.GroupBy(s => s.Colour).ToDictionary(g => g.Key, g => g.Count());
            TotalColours = $"R: {colourGroups.GetValueOrDefault("Red", 0)} B: {colourGroups.GetValueOrDefault("Black", 0)} W: {colourGroups.GetValueOrDefault("White", 0)}";

            // Calculate total milk produced by filtered livestock (Cows only)
            TotalMilk = FilteredLivestock.OfType<Cow>().Sum(cow => cow.Milk.GetValueOrDefault());

            // Calculate total wool produced by filtered livestock (Sheep only)
            TotalWool = FilteredLivestock.OfType<Sheep>().Sum(sheep => sheep.Wool.GetValueOrDefault());

            // Calculate percentage of filtered livestock compared to total livestock
            int totalStockCount = Livestock.Count;
            double avgStockPercentage = TotalStockCount > 0 ? ((double)FilteredLivestock.Count / totalStockCount * 100) : 0;
            AvgStockDisplay = $"{avgStockPercentage:F0}%";

            // Calculate average cost and weight of filtered livestock
            AvgCostDisplay = FilteredLivestock.Count > 0 ? TotalCost / FilteredLivestock.Count : 0;
            AvgWeightDisplay = FilteredLivestock.Count > 0 ? TotalWeight / FilteredLivestock.Count : 0;

            // Calculate average milk produced by filtered livestock (Cows only)
            double totalMilkCount = FilteredLivestock.OfType<Cow>().Where(cow => cow.Milk.HasValue && cow.Milk != 0).Sum(cow => cow.Milk.GetValueOrDefault());
            int milkItemCount = FilteredLivestock.OfType<Cow>().Count(cow => cow.Milk.HasValue && cow.Milk != 0);
            AvgMilkDisplay = milkItemCount > 0 ? (decimal)(totalMilkCount / milkItemCount) : 0;

            // Calculate average wool produced by filtered livestock (Sheep only)
            double totalWoolCount = FilteredLivestock.OfType<Sheep>().Where(sheep => sheep.Wool.HasValue && sheep.Wool != 0).Sum(sheep => sheep.Wool.GetValueOrDefault());
            int woolItemCount = FilteredLivestock.OfType<Sheep>().Count(sheep => sheep.Wool.HasValue && sheep.Wool != 0);
            AvgWoolDisplay = woolItemCount > 0 ? (decimal)totalWoolCount / woolItemCount : 0;

            // Round totals and averages to 2 decimal places
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

            // Calculate percentage of each colour for the averages display
            double redPercentage = totalStockCount > 0 ? (double)colourGroups.GetValueOrDefault("Red", 0) / totalStockCount * 100 : 0;
            double blackPercentage = totalStockCount > 0 ? (double)colourGroups.GetValueOrDefault("Black", 0) / totalStockCount * 100 : 0;
            double whitePercentage = totalStockCount > 0 ? (double)colourGroups.GetValueOrDefault("White", 0) / totalStockCount * 100 : 0;

            // Update average colours display with calculated percentages
            AvgColoursDisplay = $"{redPercentage:F0}% {blackPercentage:F0}% {whitePercentage:F0}%";

            // Notify property changes for all totals and averages
            OnPropertyChanged(nameof(TotalStockCount));
            OnPropertyChanged(nameof(TotalCost));
            OnPropertyChanged(nameof(TotalWeight));
            OnPropertyChanged(nameof(TotalColours));
            OnPropertyChanged(nameof(TotalMilk));
            OnPropertyChanged(nameof(TotalWool));

            OnPropertyChanged(nameof(AvgStockDisplay));
            OnPropertyChanged(nameof(AvgCostDisplay));
            OnPropertyChanged(nameof(AvgWeightDisplay));
            OnPropertyChanged(nameof(AvgMilkDisplay));
            OnPropertyChanged(nameof(AvgWoolDisplay));
            OnPropertyChanged(nameof(AvgColoursDisplay));
        }
        #endregion
        #region Property Changed Implementation
        // Event to notify the UI when a property value changes
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises the PropertyChanged event to notify the UI of property value changes.
        /// </summary>
        /// <param name="propertyName">The name of the property that changed.</param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
