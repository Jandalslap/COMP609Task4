using System;
using Microsoft.Maui.Controls;
using COMP609Task4.Models;
using COMP609Task4.ViewModels;

namespace COMP609Task4.Pages
{
    public partial class EditPage : ContentPage
    {
        private readonly Database _database;
        private readonly string _searchId;
        private readonly EditViewModel _viewModel;

        public EditPage()
        {
            InitializeComponent();
            _database = new Database();
            _viewModel = new EditViewModel();
            BindingContext = _viewModel;
        }

        public EditPage(string searchId) : this()
        {
            _searchId = searchId;
            _viewModel.SearchById(searchId);
            PopulateFields(searchId); // Call PopulateFields after searching by ID
        }

        private void SearchById_Clicked(object sender, EventArgs e)
        {
            var enteredId = IdSearchEntry.Text;
            if (!string.IsNullOrEmpty(enteredId))
            {
                _viewModel.SearchById(enteredId);
                PopulateFields(enteredId); // Call PopulateFields after searching by ID
            }
        }

        private void PopulateFields(string searchId)
        {
            var stockToEdit = _database.GetItemById(searchId);
            if (stockToEdit != null)
            {
                // Bind stockToEdit to the UI
                BindingContext = stockToEdit;
                TypeLabel.Text = stockToEdit.Type;
                IdLabel.Text = stockToEdit.Id.ToString();
                CostEntry.Text = stockToEdit.Cost.ToString();
                WeightEntry.Text = stockToEdit.Weight.ToString();
                ColourLabel.Text = stockToEdit.Colour;

                if (stockToEdit is Cow cow)
                {
                    ProduceLabel.Text = "Milk";
                    ProduceEntry.Text = cow.Milk.ToString();
                }
                else if (stockToEdit is Sheep sheep)
                {
                    ProduceLabel.Text = "Wool";
                    ProduceEntry.Text = sheep.Wool.ToString();
                }
                else
                {
                    ProduceLabel.Text = "Produce";
                    ProduceEntry.Text = "Unknown";
                }
            }
            else
            {
                Console.WriteLine("Stock item not found with ID: " + searchId);
            }
        }

        private void Update_Clicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.CommandParameter is Stock updatedStock)
            {
                updatedStock.Cost = int.Parse(CostEntry.Text);
                updatedStock.Weight = int.Parse(WeightEntry.Text);

                if (updatedStock is Cow cow)
                {
                    cow.Milk = int.Parse(ProduceEntry.Text);
                }
                else if (updatedStock is Sheep sheep)
                {
                    sheep.Wool = int.Parse(ProduceEntry.Text);
                }

                int result = _database.UpdateItem(updatedStock);

                if (result > 0)
                {
                    DisplayAlert("Success", "Stock updated successfully", "OK");
                    ClearEditForm();
                }
                else
                {
                    DisplayAlert("Error", "Failed to update stock", "OK");
                }
            }
        }

        private void Delete_Clicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.CommandParameter is Stock deletedStock)
            {
                _database.DeleteItem(deletedStock);
                DisplayAlert("Success", "Stock deleted successfully", "OK");
                ClearEditForm();
            }
        }


        private async void Home_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//MainPage");
        }

        private async void Back_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }



        private void AddNewStock_Clicked(object sender, EventArgs e)
        {
            // Check if a stock type is selected
            if (StockTypePicker.SelectedItem == null)
            {
                DisplayAlert("Error", "Please select a stock type", "OK");
                return;
            }

            // Check if a colour is selected
            if (ColourPicker.SelectedItem == null)
            {
                DisplayAlert("Error", "Please select a colour", "OK");
                return;
            }

            // Check if cost is entered
            if (string.IsNullOrWhiteSpace(AddCost.Text))
            {
                DisplayAlert("Error", "Please enter a value for cost", "OK");
                return;
            }

            // Check if weight is entered
            if (string.IsNullOrWhiteSpace(AddWeight.Text))
            {
                DisplayAlert("Error", "Please enter a value for weight", "OK");
                return;
            }

            // Parse cost and weight
            if (!int.TryParse(AddCost.Text, out int cost))
            {
                DisplayAlert("Error", "Invalid value for cost", "OK");
                return;
            }

            if (!int.TryParse(AddWeight.Text, out int weight))
            {
                DisplayAlert("Error", "Invalid value for weight", "OK");
                return;
            }

            // Get the selected stock type and colour
            string selectedStockType = StockTypePicker.SelectedItem.ToString();
            string selectedColour = ColourPicker.SelectedItem.ToString();

            // Retrieve additional field based on stock type
            int additionalField;
            if (!int.TryParse(AddProduce.Text, out additionalField))
            {
                DisplayAlert("Error", "Invalid value for additional field", "OK");
                return;
            }

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
                DisplayAlert("Success", "Stock added successfully", "OK");
                // After updating, clear the form fields
                ClearAddForm();
            }
            else
            {
                DisplayAlert("Error", "Failed to add stock", "OK");
            }
        }


        private void StockTypePicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sender is Picker picker && picker.SelectedItem != null)
            {
                if (picker.SelectedItem.ToString() == "Cow")
                {
                    AddProduceLabel.Text = "Milk";
                }
                else if (picker.SelectedItem.ToString() == "Sheep")
                {
                    AddProduceLabel.Text = "Wool";
                }
            }
        }


        private void ClearAddForm()
        {
            // Clear Stock Type Picker
            StockTypePicker.SelectedItem = null;

            // Clear Colour Picker
            ColourPicker.SelectedItem = null;

            // Clear Cost Entry
            AddCost.Text = string.Empty;

            // Clear Weight Entry
            AddWeight.Text = string.Empty;

            // Clear Additional Field Entry
            AddProduce.Text = string.Empty;
        }


        private void ClearEditForm()
        {
            TypeLabel.Text = "Type";
            IdLabel.Text = "ID";
            ColourLabel.Text = "Colour";
            CostEntry.Text = string.Empty;
            WeightEntry.Text = string.Empty;
            ProduceLabel.Text = "Produce";
            ProduceEntry.Text = string.Empty;
            IdSearchEntry.Text = string.Empty;
        }
    }
}
