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
            _viewModel = (LivestockViewModel)BindingContext;
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
            await Navigation.PushAsync(new EditPage()); // Navigate to the EditPage
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
                _viewModel.FilterStockById(enteredId);
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

            _viewModel.FilterStock(selectedType, selectedColour);
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

            _viewModel.FilterStock(selectedType, selectedColour);
        }
    }
}
