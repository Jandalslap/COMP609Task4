using Microsoft.Maui.Controls;
using COMP609Task4.ViewModels;
using System.Linq;
using System.Collections.ObjectModel;
using COMP609Task4.Models;

namespace COMP609Task4.Pages
{
    public partial class LivestockPage : ContentPage
    {
        private LivestockViewModel _viewModel;

        // Constructor for LivestockPage
        public LivestockPage()
        {
            InitializeComponent();
            _viewModel = new LivestockViewModel();
            BindingContext = _viewModel;
        }

        // Event handler for when the page appears
        protected override void OnAppearing()
        {
            base.OnAppearing();
            Console.WriteLine("OnAppearing triggered");
            _viewModel.LoadData();

            // Reset the dropdown menus
            StockTypePicker.SelectedIndex = -1; // -1 resets the dropdown
            StockColourPicker.SelectedIndex = -1; // -1 resets the dropdown
        }

        // Event handler for the Back button click event
        private async void Back_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync(); // Go back to the previous page
        }

        // Event handler for the Home button click event
        private async void Home_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//MainPage");
        }

        // Event handler for the Stock Type Picker selection change event
        private void StockTypePicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedType = StockTypePicker.SelectedItem?.ToString();
            if (selectedType == "All Stock")
            {
                selectedType = null; // Treat "All Stock" as null
            }

            // Get the currently selected colour
            var selectedColour = StockColourPicker.SelectedItem?.ToString();
            if (selectedColour == "All Colours")
            {
                selectedColour = null; // Treat "All Colours" as null
            }

            _viewModel.FilterStock(selectedType, selectedColour);
        }

        // Event handler for the Stock Colour Picker selection change event
        private void StockColourPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedColour = StockColourPicker.SelectedItem?.ToString();
            if (selectedColour == "All Colours")
            {
                selectedColour = null; // Treat "All Colours" as null
            }

            // Get the currently selected type
            var selectedType = StockTypePicker.SelectedItem?.ToString();
            if (selectedType == "All Stock")
            {
                selectedType = null; // Treat "All Stock" as null
            }

            _viewModel.FilterStock(selectedType, selectedColour);
        }

        // Event handler for the Finance button click event
        private async void Finance_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new FinancePage());
        }

        // Event handler for the Edit button click event
        private async void Edit_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new EditPage()); // Navigate to the EditPage
        }

        // Method to clear the filter selections
        private void ClearFilters()
        {
            // Reset the dropdown menus
            StockTypePicker.SelectedIndex = -1; // -1 resets the dropdown
            StockColourPicker.SelectedIndex = -1; // -1 resets the dropdown
        }

        // Event handler for the Clear Filters button click event
        private void ClearFilters_Clicked(object sender, EventArgs e)
        {
            ClearFilters();
        }
    }
}
