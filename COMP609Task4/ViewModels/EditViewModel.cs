using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using COMP609Task4.Models;
using Microsoft.Maui.Controls;

namespace COMP609Task4.ViewModels
{
    // ViewModel for managing editing operations in the application
    public class EditViewModel : INotifyPropertyChanged
    {
        #region Fields
        // Database instance for accessing and manipulating data
        private readonly Database _database;

        // Collection to hold the search results retrieved from the database
        private ObservableCollection<Stock> _searchResults;
        #endregion
        #region Properties
        // Provides access to the database instance
        public Database Database => _database;

        // Collection property to get and set search results
        public ObservableCollection<Stock> SearchResults
        {
            get => _searchResults;
            set
            {
                _searchResults = value;
                OnPropertyChanged(nameof(SearchResults)); // Notify that the SearchResults property has changed
            }
        }
        #endregion
        #region Constructor
        // Initializes a new instance of the EditViewModel class
        public EditViewModel()
        {
            _database = new Database(); // Initialize the database instance
            SearchResults = new ObservableCollection<Stock>(); // Initialize the collection for search results
        }
        #endregion
        #region Methods
        /// <summary>
        /// Searches for stock items in the database by their ID.
        /// </summary>
        /// <param name="id">The ID to search for.</param>
        public async void SearchById(string id)
        {
            // Query the database for items with the matching ID
            var result = _database.ReadItems().Where(item => item.Id.ToString() == id).ToList();

            // Check if any results were found
            if (result.Any())
            {
                // Update the search results collection with the found items
                SearchResults = new ObservableCollection<Stock>(result);
            }
            else
            {
                // Display an alert if no matching items are found
                await Application.Current.MainPage.DisplayAlert("Search Result", $"No stock found with ID: {id}", "OK");
            }
        }
        /// <summary>
        /// Adds a new stock item to the database and updates the search results.
        /// </summary>
        /// <param name="selectedStockType">The type of stock (e.g., Cow, Sheep).</param>
        /// <param name="selectedColour">The colour of the stock.</param>
        /// <param name="cost">The cost of the stock item.</param>
        /// <param name="weight">The weight of the stock item.</param>
        /// <param name="additionalField">An additional field specific to the stock type (e.g., Milk for Cows, Wool for Sheep).</param>
        /// <returns>The result of the database insert operation.</returns>
        public int AddNewStock(string selectedStockType, string selectedColour, int cost, int weight, int additionalField)
        {
            // Create a new Stock object based on the type of stock
            Stock newStock;
            if (selectedStockType == "Cow")
            {
                newStock = new Cow()
                {
                    Type = selectedStockType,
                    Colour = selectedColour,
                    Cost = cost,
                    Weight = weight,
                    Milk = additionalField // Set the additional property specific to Cow
                };
            }
            else // Assume selectedStockType is "Sheep"
            {
                newStock = new Sheep()
                {
                    Type = selectedStockType,
                    Colour = selectedColour,
                    Cost = cost,
                    Weight = weight,
                    Wool = additionalField // Set the additional property specific to Sheep
                };
            }

            // Insert the new stock into the database and get the result of the operation
            int result = _database.AddItem(newStock);

            // If the insert was successful, update the search results collection
            if (result > 0)
            {
                SearchResults.Add(newStock);
            }

            return result; // Return the result of the database insert operation
        }
        #endregion
        #region Property Changed Implementation
        // Event to notify when a property value changes
        public event PropertyChangedEventHandler PropertyChanged;

        // Method to raise the PropertyChanged event
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
