using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using COMP609Task4.Models;
using Microsoft.Maui.Controls;

namespace COMP609Task4.ViewModels
{
    // ViewModel for managing forecast operations
    public class ForecastViewModel : INotifyPropertyChanged
    {
        #region Private Members
        private readonly Database _database;

        // Observable collection that forcasted livestock are added to
        private ObservableCollection<Stock> _forecastLivestock;
        private ObservableCollection<Stock> _livestock;

        #endregion
        #region Properties
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

        public ObservableCollection<Stock> Livestock
        {
            get => _livestock;
            set
            {
                _livestock = value;
                OnPropertyChanged(nameof(Livestock));
            }
        }

        // Property to access the database instance
        public Database Database => _database;

        // Load livestock data from the database
        public void LoadData()
        {
            var livestockData = _database.ReadItems();
            _livestock = livestockData != null ? new ObservableCollection<Stock>(livestockData) : new ObservableCollection<Stock>();
            _livestock = new ObservableCollection<Stock>(_livestock);

            // Calculate totals after loading
            CalculateTotals();
        }

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
        // Properties for storing avg count, cost, tax, colours, milk, wool and income of ForecastLivestock
        private string _avgStockDisplay;
        private decimal _avgCostDisplay;
        private decimal _avgTaxDisplay;
        private string _avgColoursDisplay;
        private decimal _avgMilkDisplay;
        private decimal _avgWoolDisplay;
        private decimal _avgIncomeDisplay;

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
        #endregion
        #region Constructor
        // Constructor
        public ForecastViewModel()
        {
            _database = new Database(); // Initialize database instance
            _forecastLivestock = new ObservableCollection<Stock>(); // Initialize forecastLivestock collection
            _livestock = new ObservableCollection<Stock>(); // Initialize livestock collection


            // Rates Preferences
            // Initialize preferences with default values if they don't exist
            if (!Preferences.ContainsKey("MilkPrice"))
            {
                Preferences.Set("MilkPrice", 9.4); // Set default MilkPrice
            }

            if (!Preferences.ContainsKey("WoolPrice"))
            {
                Preferences.Set("WoolPrice", 6.2); // Set default WoolPrice
            }

            if (!Preferences.ContainsKey("TaxPrice"))
            {
                Preferences.Set("TaxPrice", 0.2); // Set default TaxPrice
            }
            // Load the preferences into the ViewModel
            _milkPrice = LoadSettings("MilkPrice");
            _woolPrice = LoadSettings("WoolPrice");
            _taxPrice = LoadSettings("TaxPrice");

            LoadData();
            FindStockAve();
            
            foreach (var stock in _forecastLivestock)
            {
                stock.TaxCalculation = CalculateTax(stock);
                stock.IncomeCalculation = CalculateIncome(stock);
            }
            CalculateTotals();
        }
        #endregion
        #region Public Methods
        // Method to find stock averages
        public Cow AveCow = new Cow();
        public Sheep AveSheep = new Sheep();
        private void FindStockAve()
        {
            foreach (var stock in _livestock)
            {                
                AveCow.Colour = "White";
                AveCow.Weight = 1;
                if (Livestock.OfType<Cow>().Count() == 0)
                {
                    AveCow.Milk = 0;
                    AveCow.Cost = 0;
                }
                else
                {
                    AveCow.Cost = (int)Math.Floor((double)Livestock.OfType<Cow>().Sum(cow => cow.Cost) / Livestock.OfType<Cow>().Count());
                    AveCow.Milk = (int)Math.Floor((double)(Livestock.OfType<Cow>().Sum(cow => cow.Milk.GetValueOrDefault()) / Livestock.OfType<Cow>().Count()));
                }
            

                AveSheep.Colour = "White";
                if (Livestock.OfType<Sheep>().Count() == 0)
                {
                    AveSheep.Wool = 0;
                    AveSheep.Cost = 0;
                }
                else
                {
                    AveSheep.Cost = (int)Math.Floor((double)Livestock.OfType<Sheep>().Sum(sheep => sheep.Cost) / Livestock.OfType<Sheep>().Count());
                    AveSheep.Wool = (int)Math.Floor((double)Livestock.OfType<Sheep>().Sum(sheep => sheep.Wool.GetValueOrDefault() / Livestock.OfType<Sheep>().Count()));
                }
                AveCow.TaxCalculation = CalculateTax(AveCow);
                AveSheep.IncomeCalculation = CalculateIncome(AveSheep);
            }
        }

        // Method to add a new stock item
        public int AddNewStock(string selectedStockType, int cost, int additionalField, int qty)
        {
            for (int i = 0; i < qty; i++) 
            { 
                // Create a new Stock object
                Stock newStock;
                if (selectedStockType == "Cow")
                {
                    newStock = new Cow()
                    {
                        // Set properties common to all stocks
                        Type = selectedStockType,
                        Colour = "White",
                        Cost = cost,
                        Weight = 1,
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
                        Colour = "White",
                        Cost = cost,
                        Weight = 1,
                        // Set additional property specific to Sheep
                        Wool = additionalField,
                        
                    };

                }

                newStock.TaxCalculation = CalculateTax(newStock);
                newStock.IncomeCalculation = CalculateIncome(newStock);

                // Insert the new stock into the database
                _forecastLivestock.Add(newStock);


                //// Update search results collection
                //SearchResults.Add(newStock); //?????

                // Update sums at botom of page.
                foreach (var stock in _forecastLivestock)
                {
                    stock.TaxCalculation = CalculateTax(stock);
                    stock.IncomeCalculation = CalculateIncome(stock);
                }
                CalculateTotals();
            }
            return 1;
        }
        // Method to add new average stock item
        public int AddAveNewStock(string selectedStockType, int qty)
        {
            for (int i = 0; i < qty; i++)
            {
                Stock newStock;

                if (selectedStockType == "Cow")
                {
                    newStock = new Cow()
                    {
                        Type = AveCow.Type, 
                        Colour = AveCow.Colour,
                        Cost = AveCow.Cost,
                        Weight = AveCow.Weight,
                        Milk = AveCow.Milk 
                    };
                }
                else // Assume selectedStockType == "Sheep"
                {
                    newStock = new Sheep()
                    {
                        Type = AveSheep.Type, 
                        Colour = AveSheep.Colour,
                        Cost = AveSheep.Cost,
                        Weight = AveSheep.Weight,
                        Wool = AveSheep.Wool 
                    };
                }

                // Calculate Income and Tax before adding
                newStock.IncomeCalculation = CalculateIncome(newStock);
                newStock.TaxCalculation = CalculateTax(newStock);

                // Add the new stock to the collection
                _forecastLivestock.Add(newStock);
            }

            // Update totals after adding all items
            CalculateTotals();
            return 1;
        }

        // Method to calculate totals
        public void CalculateTotals()
        {

            // Calculate total count, cost, and colours of filtered livestock
            TotalCost = ForecastLivestock.Sum(s => s.Cost);

            // Calculate total milk and total wool of filtered livestock
            TotalMilk = ForecastLivestock.OfType<Cow>().Sum(cow => cow.Milk.GetValueOrDefault());
            TotalWool = ForecastLivestock.OfType<Sheep>().Sum(sheep => sheep.Wool.GetValueOrDefault());

            // Calculate total income and total tax
            TotalIncome = ForecastLivestock.Sum(stock => CalculateIncome(stock));
            TotalTax = ForecastLivestock.Sum(stock => CalculateTax(stock));

            // Calculate total profit
            TotalProfit = ForecastLivestock.Sum(stock => CalculateTotalProfit(stock));

            // Format totals and averages
            TotalCost = Math.Round(TotalCost, 2);
            TotalTax = Math.Round(TotalTax, 2);
            TotalIncome = Math.Round(TotalIncome, 2);
            TotalProfit = Math.Round(TotalProfit, 2);

            AvgCostDisplay = Math.Round(AvgCostDisplay, 2);
            AvgTaxDisplay = Math.Round(AvgTaxDisplay, 2);
            AvgIncomeDisplay = Math.Round(AvgIncomeDisplay, 2);

        }
        #endregion
        #region Private Methods
        // Method to calculate total income
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

        // Method to load new Rates using Xamarin.Forms Preferences (Used Preferences as opposed to altering db)
        private decimal LoadSettings(string key)
        {
            // Retrieve the stored value associated with the specified 'key' from application preferences.
            return (decimal)Preferences.Get(key, defaultValue: 0.0);
        }
        #endregion
        #region Property Changed Implementation
        // INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); // Notify property changed
        }
        #endregion
    }

}
