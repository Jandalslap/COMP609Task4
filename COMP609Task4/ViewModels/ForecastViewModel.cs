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
        }

        // Coppied from finance view model
        public ObservableCollection<Stock> ForecastLivestock
        {
            get => _forecastLivestock;
            set
            {
                _forecastLivestock = value;
                OnPropertyChanged(nameof(_forecastLivestock));
                //CalculateTotals();
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

            
            

            return 1;
        }

        // INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); // Notify property changed
        }
    }
}
