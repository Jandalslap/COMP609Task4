using System;
using Microsoft.Maui.Controls;            
using COMP609Task4.Models;                
using COMP609Task4.ViewModels;            

namespace COMP609Task4.Pages               
{
    public partial class EditPage : ContentPage  // Define EditPage class inheriting from ContentPage
    {
        #region Private Fields
        // Private fields to store the database instance, search ID, and view model
        private readonly Database _database;    // Handles database operations
        private readonly string _searchId;      // Stores the ID for searching
        private readonly EditViewModel _viewModel; // View model for binding data to the UI
        #endregion
        #region Constructors
        // Default constructor
        public EditPage()
        {
            InitializeComponent();              // Initializes components defined in XAML
            _database = new Database();         // Instantiates the database
            _viewModel = new EditViewModel();   // Instantiates the view model
            BindingContext = _viewModel;        // Sets the view model as the data context for the page
        }

        // Constructor with a search ID parameter
        public EditPage(string searchId) : this()
        {
            _searchId = searchId;                // Store the provided search ID
            _viewModel.SearchById(searchId);     // Search the view model by ID
            PopulateFields(searchId);            // Populate the form fields with data based on the search ID
        }
        #endregion
        #region UI Initialization Methods

        // Method to populate fields with data from the database based on search ID
        private void PopulateFields(string searchId)
        {
            var stockToEdit = _database.GetItemById(searchId); // Fetch the stock item from the database by ID
            if (stockToEdit != null)              // Check if the stock item is found
            {
                // Bind stockToEdit to the UI
                BindingContext = stockToEdit;     // Set the stock item as the data context
                TypeLabel.Text = stockToEdit.Type; // Display the stock type
                IdLabel.Text = stockToEdit.Id.ToString(); // Display the stock ID
                CostEntry.Text = stockToEdit.Cost.ToString(); // Display the stock cost
                WeightEntry.Text = stockToEdit.Weight.ToString(); // Display the stock weight
                ColourEntry.Text = stockToEdit.Colour.ToString(); // Display the stock color

                // Conditional display based on the stock type (Cow or Sheep)
                if (stockToEdit is Cow cow)
                {
                    ProduceLabel.Text = "Milk";    // Set label for Cow
                    ProduceEntry.Text = cow.Milk.ToString(); // Display the milk amount for Cow
                }
                else if (stockToEdit is Sheep sheep)
                {
                    ProduceLabel.Text = "Wool";    // Set label for Sheep
                    ProduceEntry.Text = sheep.Wool.ToString(); // Display the wool amount for Sheep
                }
                else
                {
                    ProduceLabel.Text = "Produce"; // General label if type is not Cow or Sheep
                    ProduceEntry.Text = "Unknown"; // Placeholder text if type is unknown
                }
            }
            else
            {
                // Log an error message if the stock item is not found
                Console.WriteLine("Stock item not found with ID: " + searchId);
            }
        }
        #endregion
        #region Event Handlers
        // Event handler for the SearchById button click event
        private void SearchById_Clicked(object sender, EventArgs e)
        {
            var enteredId = IdSearchEntry.Text;  // Get the text from the ID search entry field
            if (!string.IsNullOrEmpty(enteredId)) // Check if the entered ID is not empty
            {
                _viewModel.SearchById(enteredId);  // Perform search by ID
                PopulateFields(enteredId);        // Populate the form fields with the search result
            }
        }

        // Event handler for the Update button click event
        private void Update_Clicked(object sender, EventArgs e)
        {
            // Validate the sender is a Button and the command parameter is of type Stock
            if (sender is Button button && button.CommandParameter is Stock updatedStock)
            {
                // Validate and parse Cost
                if (!int.TryParse(CostEntry.Text, out int cost) || cost < 0)
                {
                    DisplayAlert("Error", "Please enter a valid number for Cost.", "OK"); // Display an error alert
                    return;
                }

                // Validate and parse Weight
                if (!int.TryParse(WeightEntry.Text, out int weight) || weight < 0)
                {
                    DisplayAlert("Error", "Please enter a valid number for Weight.", "OK"); // Display an error alert
                    return;
                }

                // Validate Colour
                string colourInput = ColourEntry.Text.Trim().ToLower(); // Convert to lowercase and trim whitespace
                string colour;
                switch (colourInput)
                { // Change userinput to match database entries with a capital letter
                    case "red":
                        colour = "Red";
                        break;
                    case "white":
                        colour = "White";
                        break;
                    case "black":
                        colour = "Black";
                        break;
                    default:
                        DisplayAlert("Error", "Please enter either Red, White, or Black for Colour.", "OK"); // Display an error alert
                        return;
                }

                // Validate ProduceEntry based on Stock type (Cow or Sheep)
                if (updatedStock is Cow cow)
                {
                    if (!int.TryParse(ProduceEntry.Text, out int milk) || milk < 0)
                    {
                        DisplayAlert("Error", "Please enter a valid number for Milk.", "OK"); // Display an error alert
                        return;
                    }
                    cow.Milk = milk; // Update the milk value for Cow
                }
                else if (updatedStock is Sheep sheep)
                {
                    if (!int.TryParse(ProduceEntry.Text, out int wool) || wool < 0)
                    {
                        DisplayAlert("Error", "Please enter a valid number for Wool.", "OK"); // Display an error alert
                        return;
                    }
                    sheep.Wool = wool; // Update the wool value for Sheep
                }
                else
                {
                    // Handle unexpected type of updatedStock here if needed
                    DisplayAlert("Error", "Unsupported stock type.", "OK"); // Display an error alert for unsupported type
                    return;
                }

                // Update the stock object with validated values
                updatedStock.Cost = cost;
                updatedStock.Weight = weight;
                updatedStock.Colour = colour;

                // Perform database update
                int result = _database.UpdateItem(updatedStock);

                // Display success or failure alert based on the update result
                if (result > 0)
                {
                    DisplayAlert("Success", "Stock updated successfully", "OK");
                    ClearEditForm(); // Clear the form fields after successful update
                }
                else
                {
                    DisplayAlert("Error", "Failed to update stock", "OK");
                }
            }
        }

        // Event handler for the Delete button click event
        private async void Delete_Clicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.CommandParameter is Stock deletedStock)
            {
                // Display confirmation alert with Yes and Cancel options
                bool deleteConfirmed = await DisplayAlert("Confirm Deletion", "Are you sure you want to delete this stock item?", "Yes", "Cancel");

                if (deleteConfirmed)  // If user confirms deletion
                {
                    _database.DeleteItem(deletedStock);  // Delete the stock item from the database
                    await DisplayAlert("Success", "Stock deleted successfully", "OK");
                    ClearEditForm(); // Clear the form fields after deletion
                }
                // If cancel option was chosen, do nothing
            }
        }

        // Event handler for the Home button click event
        private async void Home_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//MainPage"); // Navigate to the main page of the app
        }

        // Event handler for the Add New Stock button click event
        private void AddNewStock_Clicked(object sender, EventArgs e)
        {
            // Validate that a stock type is selected
            if (StockTypePicker.SelectedItem == null)
            {
                DisplayAlert("Error", "Please select a stock type", "OK");
                return;
            }

            // Validate that a colour is selected
            if (ColourPicker.SelectedItem == null)
            {
                DisplayAlert("Error", "Please select a colour", "OK");
                return;
            }

            // Validate and parse cost
            if (!ValidateNonNegativeInteger(AddCost.Text, out int cost))
            {
                DisplayAlert("Error", "Please enter a valid number for cost", "OK");
                return;
            }

            // Validate and parse weight
            if (!ValidateNonNegativeInteger(AddWeight.Text, out int weight))
            {
                DisplayAlert("Error", "Please enter a valid number for weight", "OK");
                return;
            }

            // Validate and parse the additional field (Milk or Wool)
            if (!ValidateNonNegativeInteger(AddProduce.Text, out int additionalField))
            {
                DisplayAlert("Error", "Invalid value for additional field", "OK");
                return;
            }

            // Get the selected stock type and colour
            string selectedStockType = StockTypePicker.SelectedItem.ToString();
            string selectedColour = ColourPicker.SelectedItem.ToString();

            // Call the AddNewStock method from the ViewModel
            int result = _viewModel.AddNewStock(selectedStockType, selectedColour, cost, weight, additionalField);

            // Display success or failure alert based on the add result
            if (result > 0)
            {
                DisplayAlert("Success", "Stock added successfully", "OK");
                ClearAddForm(); // Clear the form fields after successful addition
            }
            else
            {
                DisplayAlert("Error", "Failed to add stock", "OK");
            }
        }

        // Event handler to clear the search by ID fields
        private void ClearSearchById_Clicked(object sender, EventArgs e)
        {
            ClearEditForm();    // Clear the edit form fields
        }

        // Event handler to clear the add new stock fields
        private void ClearAddNewStock_Clicked(object sender, EventArgs e)
        {
            ClearAddForm();     // Clear the add form fields
        }

        // Event handler for the Stock Type Picker selection change event
        private void StockTypePicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sender is Picker picker && picker.SelectedItem != null) // Check if a stock type is selected
            {
                if (picker.SelectedItem.ToString() == "Cow")
                {
                    AddProduceLabel.Text = "Milk";  // Update label to "Milk" for Cow
                }
                else if (picker.SelectedItem.ToString() == "Sheep")
                {
                    AddProduceLabel.Text = "Wool";  // Update label to "Wool" for Sheep
                }
            }
        }

        // Event handler for the Livestock button click event
        private async void Livestock_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new LivestockPage()); // Navigate to the Livestock page
        }
        #endregion
        #region Data Validation Methods
        // Method to validate that a given string is a non-negative integer
        private bool ValidateNonNegativeInteger(string input, out int value)
        {
            if (int.TryParse(input, out value)) // Try to parse the input string to an integer
            {
                if (value >= 0)                // Check if the parsed value is non-negative
                {
                    return true;               // Return true if valid
                }
            }
            return false;                      // Return false if validation fails
        }
        #endregion
        #region Utility Methods
        // Method to clear the edit form fields
        private void ClearEditForm()
        {
            TypeLabel.Text = "Type";             // Reset type label
            IdLabel.Text = "ID";                 // Reset ID label
            ColourEntry.Text = string.Empty;     // Reset colour entry field
            CostEntry.Text = string.Empty;       // Clear cost entry field
            WeightEntry.Text = string.Empty;     // Clear weight entry field
            ProduceLabel.Text = "Produce";       // Reset produce label
            ProduceEntry.Text = string.Empty;    // Clear produce entry field
            IdSearchEntry.Text = string.Empty;   // Clear search entry field
        }

        // Method to clear the add form fields
        private void ClearAddForm()
        {
            StockTypePicker.SelectedItem = null; // Clear stock type picker
            ColourPicker.SelectedItem = null;    // Clear colour picker
            AddCost.Text = string.Empty;         // Clear cost entry field
            AddWeight.Text = string.Empty;       // Clear weight entry field
            AddProduce.Text = string.Empty;      // Clear produce entry field
        }
        #endregion
    }
}
