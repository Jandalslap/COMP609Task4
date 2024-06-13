using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace COMP609Task4.ViewModels
{
    internal class FinanceViewModel : INotifyPropertyChanged
    {
        // Database instance for accessing livestock data
        private readonly Database _database;

        // Observable collections for holding all livestock and filtered livestock data
        private ObservableCollection<Stock> _livestock;
        private ObservableCollection<Stock> _filteredLivestock;

        // Properties

        // Property to access the database instance
        public Database Database => _database;

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

        public ICommand UpdateCommand { get; }

        // Constructor
        public FinanceViewModel()
        {
            _database = new Database();
            LoadData();
            FilteredLivestock = new ObservableCollection<Stock>(Livestock);
            CalculateTotals();
            // Initialize with stored starting values
            _milkPrice = 9.4m;
            _woolPrice = 6.2m;
            _taxPrice = 0.02m;
            UpdateCommand = new Command(OnUpdate);
        }

        // Load livestock data from the database
        public void LoadData()
        {
            var livestockData = _database.ReadItems();
            Livestock = livestockData != null ? new ObservableCollection<Stock>(livestockData) : new ObservableCollection<Stock>();
            FilteredLivestock = new ObservableCollection<Stock>(Livestock);

            // Calculate totals after loading
            CalculateTotals();
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

            // Calculate TaxCalculation and IncomeCalculation properties for each filtered livestock
            foreach (var stock in filteredItems)
            {
                stock.TaxCalculation = CalculateTax(stock);
                stock.IncomeCalculation = CalculateIncome(stock);
            }

            // Update the FilteredLivestock collection after filtering
            FilteredLivestock = new ObservableCollection<Stock>(filteredItems);
        }


        // Totals properties      
        private int _totalStockCount;
        private decimal _totalCost;
        private decimal _totalTax;
        private string _totalColours;
        private decimal _totalMilk;
        private decimal _totalWool;
        private decimal _totalIncome;
        private decimal _totalProfit;

        // Properties for storing total profit, count, cost, tax, colours, milk, wool and income of filtered livestock
        public decimal TotalProfit
        {
            get => _totalProfit;
            set
            {
                _totalProfit = value;
                OnPropertyChanged(nameof(TotalProfit));
            }
        }

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

        public decimal TotalTax
        {
            get => _totalTax;
            set
            {
                _totalTax = value;
                OnPropertyChanged(nameof(TotalTax));
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

        public decimal TotalIncome
        {
            get => _totalIncome;
            set
            {
                _totalIncome = value;
                OnPropertyChanged(nameof(TotalIncome));
            }
        }

        // Avg properties
        private string _avgStockDisplay;
        private decimal _avgCostDisplay;
        private decimal _avgTaxDisplay;
        private string _avgColoursDisplay;
        private decimal _avgMilkDisplay;
        private decimal _avgWoolDisplay;
        private decimal _avgIncomeDisplay;

        // Properties for storing avg count, cost, tax, colours, milk, wool and income of filtered livestock
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

        public decimal AvgTaxDisplay
        {
            get => _avgTaxDisplay;
            set
            {
                _avgTaxDisplay = value;
                OnPropertyChanged(nameof(AvgTaxDisplay));
            }
        }

        public decimal AvgIncomeDisplay
        {
            get => _avgIncomeDisplay;
            set
            {
                _avgIncomeDisplay = value;
                OnPropertyChanged(nameof(AvgIncomeDisplay));
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
        public void CalculateTotals()
        {
            int totalStockCount = Livestock.Count;

            // Calculate total count, cost, and colours of filtered livestock
            TotalStockCount = FilteredLivestock.Count;
            TotalCost = FilteredLivestock.Sum(s => s.Cost);

            var colourGroups = FilteredLivestock.GroupBy(s => s.Colour).ToDictionary(g => g.Key, g => g.Count());
            TotalColours = $"R: {colourGroups.GetValueOrDefault("Red", 0)} B: {colourGroups.GetValueOrDefault("Black", 0)} W: {colourGroups.GetValueOrDefault("White", 0)}";

            // Calculate total milk and total wool of filtered livestock
            TotalMilk = FilteredLivestock.OfType<Cow>().Sum(cow => cow.Milk.GetValueOrDefault());
            TotalWool = FilteredLivestock.OfType<Sheep>().Sum(sheep => sheep.Wool.GetValueOrDefault());

            // Calculate total income and total tax
            TotalIncome = FilteredLivestock.Sum(stock => CalculateIncome(stock));
            TotalTax = FilteredLivestock.Sum(stock => CalculateTax(stock));

            // Calculate total profit
            TotalProfit = FilteredLivestock.Sum(stock => CalculateTotalProfit(stock));

            // Calculate average stock percentage and convert to string
            double avgStockPercentage = TotalStockCount > 0 ? ((double)FilteredLivestock.Count / totalStockCount * 100) : 0;
            AvgStockDisplay = $"{avgStockPercentage:F0}%";

            // Calculate average cost
            AvgCostDisplay = FilteredLivestock.Count > 0 ? TotalCost / FilteredLivestock.Count : 0;

            // Calculate average tax
            AvgTaxDisplay = FilteredLivestock.Count > 0 ? TotalTax / FilteredLivestock.Count : 0;

            // calculate average income
            AvgIncomeDisplay = FilteredLivestock.Count > 0 ? TotalIncome / FilteredLivestock.Count : 0;

            // Calculate average milk
            double totalMilkCount = FilteredLivestock.OfType<Cow>().Where(cow => cow.Milk.HasValue && cow.Milk != 0).Sum(cow => cow.Milk.GetValueOrDefault());
            int milkItemCount = FilteredLivestock.OfType<Cow>().Count(cow => cow.Milk.HasValue && cow.Milk != 0);
            AvgMilkDisplay = milkItemCount > 0 ? (decimal)(totalMilkCount / milkItemCount) : 0;

            // Calculate average wool
            double totalWoolCount = FilteredLivestock.OfType<Sheep>().Where(sheep => sheep.Wool.HasValue && sheep.Wool != 0).Sum(sheep => sheep.Wool.GetValueOrDefault());
            int woolItemCount = FilteredLivestock.OfType<Sheep>().Count(sheep => sheep.Wool.HasValue && sheep.Wool != 0);
            AvgWoolDisplay = woolItemCount > 0 ? (decimal)totalWoolCount / woolItemCount : 0;

            // Calculate percentage of each color for the averages line
            double redPercentage = totalStockCount > 0 ? (double)colourGroups.GetValueOrDefault("Red", 0) / totalStockCount * 100 : 0;
            double blackPercentage = totalStockCount > 0 ? (double)colourGroups.GetValueOrDefault("Black", 0) / totalStockCount * 100 : 0;
            double whitePercentage = totalStockCount > 0 ? (double)colourGroups.GetValueOrDefault("White", 0) / totalStockCount * 100 : 0;

            // Concatenate total colours with percentages for the averages line
            AvgColoursDisplay = $"{redPercentage:F0}% {blackPercentage:F0}% {whitePercentage:F0}%";

            // Format totals and averages
            TotalCost = Math.Round(TotalCost, 2);
            TotalTax = Math.Round(TotalTax, 2);
            TotalIncome = Math.Round(TotalIncome, 2);
            TotalProfit = Math.Round(TotalProfit, 2);


            AvgCostDisplay = Math.Round(AvgCostDisplay, 2);
            AvgTaxDisplay = Math.Round(AvgTaxDisplay, 2);
            AvgIncomeDisplay = Math.Round(AvgIncomeDisplay, 2);

            // Store original values
            _originalTotalCost = TotalCost;
            _originalTotalTax = TotalTax;
            _originalTotalIncome = TotalIncome;
            _originalTotalProfit = TotalProfit;
            _originalTotalMilk = TotalMilk;
            _originalTotalWool = TotalWool;
            _originalAvgCostDisplay = AvgCostDisplay;
            _originalAvgTaxDisplay = AvgTaxDisplay;
            _originalAvgIncomeDisplay = AvgIncomeDisplay;
            _originalAvgMilkDisplay = AvgMilkDisplay;
            _originalAvgWoolDisplay = AvgWoolDisplay;
        }

        // Metod to calculate total income
        private decimal CalculateIncome(Stock stock)
        {
            decimal income = 0;

            if (stock is Cow cow && cow.Milk.HasValue)
            {
                income += cow.Milk.Value * (decimal)MilkPrice;
            }

            if (stock is Sheep sheep && sheep.Wool.HasValue)
            {
                income += sheep.Wool.Value * (decimal)WoolPrice;
            }

            return income;
        }

        // Metod to calculate total tax
        private decimal CalculateTax(Stock stock)
        {
            return stock.Cost * (decimal)TaxPrice;
        }

        // Metod to calculate total profit
        private decimal CalculateTotalProfit(Stock stock)
        {
            decimal income = CalculateIncome(stock);
            decimal tax = CalculateTax(stock);
            decimal cost = stock.Cost;

            decimal totalProfit = income - tax - cost;

            return totalProfit;
        }

        // Rates properties
        private decimal _milkPrice;
        private decimal _woolPrice;
        private decimal _taxPrice;

        public string MilkPriceText { get; set; }
        public string WoolPriceText { get; set; }
        public string TaxPriceText { get; set; }

        public decimal MilkPrice
        {
            get => _milkPrice;
            set
            {
                _milkPrice = value;
                OnPropertyChanged(nameof(MilkPrice));
            }
        }

        public decimal WoolPrice
        {
            get => _woolPrice;
            set
            {
                _woolPrice = value;
                OnPropertyChanged(nameof(WoolPrice));
            }
        }

        public decimal TaxPrice
        {
            get => _taxPrice;
            set
            {
                _taxPrice = value;
                OnPropertyChanged(nameof(TaxPrice));
            }
        }

        // Method to update Rates
        private void OnUpdate()
        {
            // Convert string inputs to decimals before assigning to properties
            decimal milkPrice;
            decimal woolPrice;
            decimal taxPrice;
            if (decimal.TryParse(MilkPriceText, out milkPrice))
            {
                MilkPrice = milkPrice;
            }

            if (decimal.TryParse(WoolPriceText, out woolPrice))
            {
                WoolPrice = woolPrice;
            }

            if (decimal.TryParse(TaxPriceText, out taxPrice))
            {
                TaxPrice = taxPrice;
            }

            // Save the updated prices to settings
            SaveSettings("MilkPrice", (double)MilkPrice);
            SaveSettings("WoolPrice", (double)WoolPrice);
            SaveSettings("TaxPrice", (double)TaxPrice);

            // Recalculate totals and averages
            CalculateTotals();
        }

        // Method to save new Rates
        private void SaveSettings(string key, double value)
        {
            Preferences.Set(key, value);
        }

        // Method to load new Rates
        private decimal LoadSettings(string key)
        {
            return (decimal)Preferences.Get(key, defaultValue: 0.0);
        }

        // Event handler for PropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        // OnPropertyChanged method for updating property changes
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private decimal _originalTotalCost;
        private decimal _originalTotalTax;
        private decimal _originalTotalIncome;
        private decimal _originalTotalProfit;
        private decimal _originalTotalMilk;
        private decimal _originalTotalWool;
        private decimal _originalAvgCostDisplay;
        private decimal _originalAvgTaxDisplay;
        private decimal _originalAvgIncomeDisplay;
        private decimal _originalAvgMilkDisplay;
        private decimal _originalAvgWoolDisplay;

        // Method for recalculation for different time periods
        public void RecalculateTotalsBasedOnPeriod(string selectedPeriod)
        {
            // Determine the multiplier based on the selected period
            int multiplier = 1; // Default to daily
            if (selectedPeriod == "Daily")
            {
                ResetToOriginalValues();
            }
            else if (selectedPeriod == "Weekly")
            {
                multiplier = 7;
            }
            else if (selectedPeriod == "Monthly")
            {
                multiplier = 30;
            }
            else if (selectedPeriod == "Annually")
            {
                multiplier = 365;
            }

            // Recalculate totals with the multiplier
            TotalCost = _originalTotalCost * multiplier;
            TotalTax = _originalTotalTax * multiplier;
            TotalIncome = _originalTotalIncome * multiplier;
            TotalProfit = _originalTotalProfit * multiplier;
            TotalMilk = _originalTotalMilk * multiplier;
            TotalWool = _originalTotalWool * multiplier;

            // Recalculate averages with the multiplier
            AvgCostDisplay = _originalAvgCostDisplay * multiplier;
            AvgTaxDisplay = _originalAvgTaxDisplay * multiplier;
            AvgIncomeDisplay = _originalAvgIncomeDisplay * multiplier;
            AvgMilkDisplay = _originalAvgMilkDisplay * multiplier;
            AvgWoolDisplay = _originalAvgWoolDisplay * multiplier;

            // Update the UI bindings by raising PropertyChanged for affected properties
            OnPropertyChanged(nameof(TotalCost));
            OnPropertyChanged(nameof(TotalTax));
            OnPropertyChanged(nameof(TotalIncome));
            OnPropertyChanged(nameof(TotalProfit));
            OnPropertyChanged(nameof(TotalMilk));
            OnPropertyChanged(nameof(TotalWool));
            OnPropertyChanged(nameof(AvgCostDisplay));
            OnPropertyChanged(nameof(AvgTaxDisplay));
            OnPropertyChanged(nameof(AvgIncomeDisplay));
            OnPropertyChanged(nameof(AvgMilkDisplay));
            OnPropertyChanged(nameof(AvgWoolDisplay));
        }

        // Method to reset values for original time period
        private void ResetToOriginalValues()
        {
            TotalCost = _originalTotalCost;
            TotalTax = _originalTotalTax;
            TotalIncome = _originalTotalIncome;
            TotalProfit = _originalTotalProfit;
            AvgCostDisplay = _originalAvgCostDisplay;
            AvgTaxDisplay = _originalAvgTaxDisplay;
            AvgIncomeDisplay = _originalAvgIncomeDisplay;

            OnPropertyChanged(nameof(TotalCost));
            OnPropertyChanged(nameof(TotalTax));
            OnPropertyChanged(nameof(TotalIncome));
            OnPropertyChanged(nameof(TotalProfit));
            OnPropertyChanged(nameof(AvgCostDisplay));
            OnPropertyChanged(nameof(AvgTaxDisplay));
            OnPropertyChanged(nameof(AvgIncomeDisplay));
        }

    }
}
