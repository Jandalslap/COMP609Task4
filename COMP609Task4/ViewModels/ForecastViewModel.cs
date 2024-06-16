using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using COMP609Task4.Models;
using Microsoft.Maui.Controls;

namespace COMP609Task4.ViewModels
{
    // ViewModel for managing editing operations
    public class ForecastViewModel : INotifyPropertyChanged
    {
        // Observable collection that forcasted livestock are added to
        private ObservableCollection<Stock> _forecastLivestock;

        // Databse connection/instance not required for this page

        // Constructor
        public ForecastViewModel()
        {
            //_database = new Database(); // Initialize database instance
            _forecastLivestock = new ObservableCollection<Stock>(); // Initialize search results collection
            // Just for testing, adds 6 cows to the observable collection
            for (int i = 0; i < 5; i++)
            {
                Stock newStock;
                newStock = new Cow()
                {
                    // Set properties common to all stocks
                    Type = "Cow",
                    Colour = "Red",
                    Cost = 33,
                    Weight = 66,
                    // Set additional property specific to Cow
                    Milk = 22

                };
                _forecastLivestock.Add(newStock);
            }
            CalculateTotals();
        }

        // Coppied from finance view model
        public ObservableCollection<Stock> ForecastLivestock
        {
            get => _forecastLivestock;
            set
            {
                _forecastLivestock = value;
                OnPropertyChanged(nameof(_forecastLivestock));
                CalculateTotals();
            }
        }



        // Method to add a new stock item
        public int AddNewStock(string selectedStockType, string selectedColour, int cost, int weight, int additionalField)
        {
            // Create a new Stock object
            Stock newStock;
            if (selectedStockType == "Cow")
            {
                newStock = new Cow()
                {
                    // Set properties common to all stocks
                    Type = selectedStockType,
                    Colour = selectedColour,
                    Cost = cost,
                    Weight = weight,
                    // Set additional property specific to Cow
                    Milk = additionalField
                };
            }
            else // Assume selectedStockType == "Sheep"
            {
                newStock = new Sheep()
                {
                    // Set properties common to all stocks
                    Type = selectedStockType,
                    Colour = selectedColour,
                    Cost = cost,
                    Weight = weight,
                    // Set additional property specific to Sheep
                    Wool = additionalField
                };
            }

            // Insert the new stock into the database
            _forecastLivestock.Add(newStock);


            //// Update search results collection
            //SearchResults.Add(newStock); //?????

            // Update sums at botom of page.
            CalculateTotals();            
            

            return 1;
        }

        // INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); // Notify property changed
        }
        
        #region avgProps
        // Avg properties
        private string _avgStockDisplay;
        private decimal _avgCostDisplay;
        private decimal _avgTaxDisplay;
        private string _avgColoursDisplay;
        private decimal _avgMilkDisplay;
        private decimal _avgWoolDisplay;
        private decimal _avgIncomeDisplay;

        // Properties for storing avg count, cost, tax, colours, milk, wool and income of ForecastLivestock
        
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
        #endregion

        #region totProps
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
        #endregion


        public void CalculateTotals()
        {
            int totalStockCount = ForecastLivestock.Count;

            // Calculate total count, cost, and colours of filtered livestock
            TotalStockCount = ForecastLivestock.Count;
            TotalCost = ForecastLivestock.Sum(s => s.Cost);

            var colourGroups = ForecastLivestock.GroupBy(s => s.Colour).ToDictionary(g => g.Key, g => g.Count());
            TotalColours = $"R: {colourGroups.GetValueOrDefault("Red", 0)} B: {colourGroups.GetValueOrDefault("Black", 0)} W: {colourGroups.GetValueOrDefault("White", 0)}";

            // Calculate total milk and total wool of filtered livestock
            TotalMilk = ForecastLivestock.OfType<Cow>().Sum(cow => cow.Milk.GetValueOrDefault());
            TotalWool = ForecastLivestock.OfType<Sheep>().Sum(sheep => sheep.Wool.GetValueOrDefault());

            // Calculate total income and total tax
            TotalIncome = ForecastLivestock.Sum(stock => CalculateIncome(stock));
            TotalTax = ForecastLivestock.Sum(stock => CalculateTax(stock));

            // Calculate total profit
            TotalProfit = ForecastLivestock.Sum(stock => CalculateTotalProfit(stock));

            // Calculate average stock percentage and convert to string
            double avgStockPercentage = TotalStockCount > 0 ? ((double)ForecastLivestock.Count / _totalStockCount * 100) : 0;
            AvgStockDisplay = $"{avgStockPercentage:F0}%";

            // Calculate average cost
            AvgCostDisplay = ForecastLivestock.Count > 0 ? TotalCost / ForecastLivestock.Count : 0;

            // Calculate average tax
            AvgTaxDisplay = ForecastLivestock.Count > 0 ? TotalTax / ForecastLivestock.Count : 0;

            // calculate average income
            AvgIncomeDisplay = ForecastLivestock.Count > 0 ? TotalIncome / ForecastLivestock.Count : 0;

            // Calculate average milk
            double totalMilkCount = ForecastLivestock.OfType<Cow>().Where(cow => cow.Milk.HasValue && cow.Milk != 0).Sum(cow => cow.Milk.GetValueOrDefault());
            int milkItemCount = ForecastLivestock.OfType<Cow>().Count(cow => cow.Milk.HasValue && cow.Milk != 0);
            AvgMilkDisplay = milkItemCount > 0 ? (decimal)(totalMilkCount / milkItemCount) : 0;

            // Calculate average wool
            double totalWoolCount = ForecastLivestock.OfType<Sheep>().Where(sheep => sheep.Wool.HasValue && sheep.Wool != 0).Sum(sheep => sheep.Wool.GetValueOrDefault());
            int woolItemCount = ForecastLivestock.OfType<Sheep>().Count(sheep => sheep.Wool.HasValue && sheep.Wool != 0);
            AvgWoolDisplay = woolItemCount > 0 ? (decimal)totalWoolCount / woolItemCount : 0;

            // Calculate percentage of each color for the averages line
            double redPercentage = _totalStockCount > 0 ? (double)colourGroups.GetValueOrDefault("Red", 0) / _totalStockCount * 100 : 0;
            double blackPercentage = _totalStockCount > 0 ? (double)colourGroups.GetValueOrDefault("Black", 0) / _totalStockCount * 100 : 0;
            double whitePercentage = _totalStockCount > 0 ? (double)colourGroups.GetValueOrDefault("White", 0) / _totalStockCount * 100 : 0;

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
    }
}
