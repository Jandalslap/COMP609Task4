using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using COMP609Task4.Models;
using Microsoft.Maui.Controls;

namespace COMP609Task4.ViewModels
{
    // ViewModel for managing editing operations
    public class EditViewModel : INotifyPropertyChanged
    {
        // Database instance for accessing data
        private readonly Database _database;

        // Collection to hold search results
        private ObservableCollection<Stock> _searchResults;

        // Property to access the database instance
        public Database Database => _database;

        // Collection property to hold search results
        public ObservableCollection<Stock> SearchResults
        {
            get => _searchResults;
            set
            {
                _searchResults = value;
                OnPropertyChanged(nameof(SearchResults)); // Notify property changed
            }
        }

        // Constructor
        public EditViewModel()
        {
            _database = new Database(); // Initialize database instance
            SearchResults = new ObservableCollection<Stock>(); // Initialize search results collection
        }

        // Method to search for items by ID
        public async void SearchById(string id)
        {
            // Query the database for items with matching ID
            var result = _database.ReadItems().Where(item => item.Id.ToString() == id).ToList();

            // Check if any results were found
            if (result.Any())
            {
                // Update search results collection
                SearchResults = new ObservableCollection<Stock>(result);
            }
            else
            {
                // Show an alert if no matching items are found
                await Application.Current.MainPage.DisplayAlert("Search Result", $"No stock found with ID: {id}", "OK");
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
            int result = _database.AddItem(newStock);

            if (result > 0)
            {
                // Update search results collection
                SearchResults.Add(newStock);
            }

            return result;
        }

        // INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); // Notify property changed
        }
    }
}
