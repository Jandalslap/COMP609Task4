using Microsoft.Maui.Controls;
using COMP609Task4.ViewModels;
using System.Linq;
using System.Collections.ObjectModel;
using COMP609Task4.Models;

namespace COMP609Task4.Pages;

public partial class FinancePage : ContentPage
{
    #region Private Members
    private FinanceViewModel _viewModel;
    #endregion
    #region Constructor
    // Constructor for FinancePage
    public FinancePage()
    {
        InitializeComponent();
        _viewModel = new FinanceViewModel();
        BindingContext = _viewModel;
    }
    #endregion
    #region Lifecycle Methods
    // Event handler for when the page appears
    protected override void OnAppearing()
    {
        base.OnAppearing();
        Console.WriteLine("OnAppearing triggered");
        _viewModel.LoadData();

        // Reset the dropdown menus
        StockTypePicker.SelectedIndex = 0; 
        StockColourPicker.SelectedIndex = 0;
        TimePeriodPicker.SelectedIndex = 0;
    }

    #endregion
    #region Event Handlers
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

        // Trigger recalculation
        string selectedPeriod = (TimePeriodPicker.SelectedItem as string);
        _viewModel.RecalculateTotalsBasedOnPeriod(selectedPeriod);
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

        // Trigger recalculation
        string selectedPeriod = (TimePeriodPicker.SelectedItem as string);
        _viewModel.RecalculateTotalsBasedOnPeriod(selectedPeriod);
    }

    // Event handler for the Time Period Picker selection change event
    private void TimePeriodPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        // Retrieve the selected period from the dropdown
        string selectedPeriod = TimePeriodPicker.SelectedItem as string;
        if (_viewModel != null)
        {
            // Update the ViewModel's SelectedPeriod property
            _viewModel.SelectedPeriod = selectedPeriod;
            // Recalculate the totals based on the new period
            _viewModel.RecalculateTotalsBasedOnPeriod(selectedPeriod);
        }
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
        StockTypePicker.SelectedIndex = 0; 
        StockColourPicker.SelectedIndex = 0; 
        TimePeriodPicker.SelectedIndex = 0; 
    }

    // Event handler for the Clear Filters button click event
    private void ClearFilters_Clicked(object sender, EventArgs e)
    {
        ClearFilters();
    }

    // Event handler for the Forecast button click event
    private async void Forecast_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ForecastPage()); // Navigate to the ForecastPage
    }

    // Event handler for the Update Rates button click event
    private void UpdateButton_Clicked(object sender, EventArgs e)
    {
        // Try parse text entry into decimal else fallback value is 0
        decimal milkPrice = decimal.TryParse(MilkPriceEntry.Text, out milkPrice) ? milkPrice : 0;
        decimal woolPrice = decimal.TryParse(WoolPriceEntry.Text, out woolPrice) ? woolPrice : 0;
        decimal taxPrice = decimal.TryParse(TaxPriceEntry.Text, out taxPrice) ? taxPrice : 0;
    }
    #endregion

    public  decimal[] GetAvgs(String stock)
    {
        return _viewModel.GetAvgCostWeightProd(stock);
    }
}