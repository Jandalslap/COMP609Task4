using System;
using Microsoft.Maui.Controls;
using COMP609Task4.Models;

namespace COMP609Task4.Pages
{
    public partial class EditPage : ContentPage
    {
        private readonly string _searchId;
        private readonly Database _database;

        public EditPage(string searchId)
        {
            InitializeComponent();
            _searchId = searchId;
            _database = new Database(); // Initialize your database instance here

            // Populate the fields with data based on the search ID
            PopulateFields();
        }

        private void PopulateFields()
        {
            // Retrieve the stock item from the database based on the search ID
            var stockToEdit = _database.GetItemById(_searchId);

            if (stockToEdit != null)
            {
                // Set the BindingContext for the entire page or the relevant section
                this.BindingContext = stockToEdit;

                // Populate the fields with data from the retrieved stock item
                TypeLabel.Text = stockToEdit.Type;
                IdLabel.Text = stockToEdit.Id.ToString(); // Convert integer ID to string
                CostEntry.Text = stockToEdit.Cost.ToString();
                WeightEntry.Text = stockToEdit.Weight.ToString();
                ColourLabel.Text = stockToEdit.Colour;

                // Check the type of stock to set the produce label and field
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
                // Handle case where stock item is not found
                Console.WriteLine("Stock item not found with ID: " + _searchId);
            }
        }


        private void Update_Clicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                var commandParameter = button.CommandParameter;
                Console.WriteLine("Button Clicked");
                Console.WriteLine($"CommandParameter: {commandParameter}");

                // Get the Stock object bound to the button that was clicked
                var updatedStock = commandParameter as Stock;

                if (updatedStock != null)
                {
                    // Update the stock properties from the UI fields
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

                    // Update the database with the changes made to the Stock object
                    int result = _database.UpdateItem(updatedStock);

                    if (result > 0)
                    {
                        DisplayAlert("Success", "Stock updated successfully", "OK");
                    }
                    else
                    {
                        DisplayAlert("Error", "Failed to update stock", "OK");
                    }
                }
                else
                {
                    Console.WriteLine("CommandParameter is not a Stock object.");
                }
            }
            else
            {
                Console.WriteLine("Sender is not a Button.");
            }
        }

        private void Delete_Clicked(object sender, EventArgs e)
        {
            // Get the Stock object bound to the button that was clicked
            var deletedStock = (sender as Button)?.CommandParameter as Stock;

            if (deletedStock != null)
            {
                // Delete the item from the database
                _database.DeleteItem(deletedStock);
                // Optionally, you can display a message or perform other actions after the deletion
                DisplayAlert("Success", "Stock deleted successfully", "OK");

                // Clear the edit form
                ClearEditForm();
            }
        }

        private void ClearEditForm()
        {
            // Reset the values of the form fields to their default state
            TypeLabel.Text = "Type";
            IdLabel.Text = "ID";
            ColourLabel.Text = "Colour"; ;
            CostEntry.Text = string.Empty;
            WeightEntry.Text = string.Empty;
            ProduceStackLayout.IsVisible = false;
        }


        private async void Home_Clicked(object sender, EventArgs e)
        {
            // Add your home button logic here
            await Shell.Current.GoToAsync("//MainPage");
        }

        private async void Back_Clicked(object sender, EventArgs e)
        {
            // Add your back button logic here
            await Navigation.PopAsync();
        }

        private void AddNewStock_Clicked(object sender, EventArgs e)
        {

            // Set default values or clear fields
            TypeLabel.Text = "Stock Type:";
            IdLabel.Text = "Stock ID:";
            ColourLabel.Text = "Colour";
            CostEntry.Placeholder = "Cost";
            WeightEntry.Placeholder = "Weight";
            ProduceLabel.Text = "Produce";
            ProduceEntry.Placeholder = "Produce";
        }

        //private void SaveNewStock_Clicked(object sender, EventArgs e)
        //{
        //    // Create a new Stock object based on the form input
        //    var newStockType = NewStockTypePicker.SelectedItem.ToString();
        //    var newStockColour = NewStockColourPicker.SelectedItem.ToString();
        //    var newStockCost = int.Parse(NewStockCostEntry.Text);
        //    var newStockWeight = int.Parse(NewStockWeightEntry.Text);
        //    var newStockProduce = NewStockProduceEntry.Text;

        //    Stock newStock;
        //    if (newStockType == "Cow")
        //    {
        //        newStock = new Cow
        //        {
        //            Type = "Cow",
        //            Colour = newStockColour,
        //            Cost = newStockCost,
        //            Weight = newStockWeight,
        //            Milk = int.Parse(newStockProduce)
        //        };
        //    }
        //    else
        //    {
        //        newStock = new Sheep
        //        {
        //            Type = "Sheep",
        //            Colour = newStockColour,
        //            Cost = newStockCost,
        //            Weight = newStockWeight,
        //            Wool = int.Parse(newStockProduce)
        //        };
        //    }

        //    // Add the new stock item to the database
        //    int result = _database.AddItem(newStock);

        //    if (result > 0)
        //    {
        //        DisplayAlert("Success", "New stock added successfully", "OK");
        //        // Hide the form and reset input fields
        //        NewStockForm.IsVisible = false;
        //        NewStockTypePicker.SelectedIndex = -1;
        //        NewStockColourPicker.SelectedIndex = -1;
        //        NewStockCostEntry.Text = string.Empty;
        //        NewStockWeightEntry.Text = string.Empty;
        //        NewStockProduceEntry.Text = string.Empty;
        //    }
        //    else
        //    {
        //        DisplayAlert("Error", "Failed to add new stock", "OK");
        //    }
        //}

        //private void ShowAddNewStockForm(object sender, EventArgs e)
        //{
        //    // Show the form for adding new stock
        //    NewStockForm.IsVisible = true;
        //}

        //private void OnTypeSelected(object sender, EventArgs e)
        //{
        //    if (NewStockTypePicker.SelectedItem?.ToString() == "Cow")
        //    {
        //        NewStockProduceEntry.Placeholder = "Milk";
        //        NewStockProduceEntry.IsVisible = true;
        //    }
        //    else if (NewStockTypePicker.SelectedItem?.ToString() == "Sheep")
        //    {
        //        NewStockProduceEntry.Placeholder = "Wool";
        //        NewStockProduceEntry.IsVisible = true;
        //    }
        //    else
        //    {
        //        NewStockProduceEntry.IsVisible = false;
        //    }
        //}
    }
}
