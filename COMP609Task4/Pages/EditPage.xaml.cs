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
                ColourEntry.Text = stockToEdit.Colour;

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
                    updatedStock.Colour = ColourEntry.Text;

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
    }
}
