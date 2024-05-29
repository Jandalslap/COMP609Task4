using Microsoft.Maui.Controls;
using COMP609Task4.ViewModels;

namespace COMP609Task4.Pages
{
    public partial class LivestockPage : ContentPage
    {
        private LivestockViewModel _viewModel;

        public LivestockPage()
        {
            InitializeComponent();
            _viewModel = new LivestockViewModel();
            BindingContext = _viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.LoadData();

            // Reset the search box and dropdown menus
            IdSearchEntry.Text = string.Empty;
            StockTypePicker.SelectedIndex = -1; // Assuming -1 resets the dropdown
            StockColourPicker.SelectedIndex = -1; // Assuming -1 resets the dropdown
        }

        private async void Back_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync(); // Go back to the previous page
        }

        private async void Cost_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new FinancePage()); // Navigate to the FinancePage
        }

        private async void Edit_Clicked(object sender, EventArgs e)
        {
            try
            {
                // Check if _viewModel is null
                if (_viewModel == null)
                {
                    Console.WriteLine("_viewModel is null");
                    return; // Exit the method if _viewModel is null
                }

                // Check if a search has been made and a valid ID is entered
                if (_viewModel.IsSearchMade && !string.IsNullOrEmpty(_viewModel.SearchId))
                {
                    // Navigate to the EditPage and pass only the SearchId
                    await Navigation.PushAsync(new EditPage(_viewModel.SearchId));
                }
                else
                {
                    // Handle case where no search has been made or the ID entered is empty
                    Console.WriteLine("No search made or ID is empty");
                }
            }
            catch (Exception ex)
            {
                // Log any exceptions to the console or debug output
                Console.WriteLine("Error in Edit_Clicked: " + ex.Message);
            }
        }

        private async void Home_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//MainPage");
        }

        private void SearchById_Clicked(object sender, EventArgs e)
        {
            var enteredId = IdSearchEntry.Text;
            if (!string.IsNullOrEmpty(enteredId))
            {
                // Reset the selected indices of the dropdown menus to show "All Stock" and "All Colours"
                StockTypePicker.SelectedIndex = 0;
                StockColourPicker.SelectedIndex = 0;

                _viewModel.FilterStockById(enteredId);

                // Store the entered ID in the SearchId property
                _viewModel.SearchId = enteredId;

                // Show the Edit button only when a search is made
                EditButton.IsVisible = _viewModel.IsSearchMade;

            }
        }

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

            // Clear the search box
            IdSearchEntry.Text = string.Empty;
            _viewModel.FilterStock(selectedType, selectedColour);

            // Hide the Edit button when filters are applied
            EditButton.IsVisible = false;
        }

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

            // Clear the search box
            IdSearchEntry.Text = string.Empty;
            _viewModel.FilterStock(selectedType, selectedColour);

            // Hide the Edit button when filters are applied
            EditButton.IsVisible = false;
        }
    }
}
