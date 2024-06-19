//using Windows.Storage.Search; //Was this added automaticly?

namespace COMP609Task4.Pages;

public partial class ForecastPage : ContentPage
{
    private readonly Database _database;
    //private readonly string _searchId;
    private readonly ForecastViewModel _viewModel;
    public ForecastPage()
    {
        InitializeComponent();
        _database = new Database();
        _viewModel = new ForecastViewModel();
        BindingContext = _viewModel;
    }
    // Event handler for when the page appears
    protected override void OnAppearing()
    {
        base.OnAppearing();
        Console.WriteLine("OnAppearing triggered");
        _viewModel.LoadData();

        // Reset the dropdown menus
        StockTypePicker.SelectedIndex = -1;
    }
    private async void Back_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync(); // Go back to the previous page
    }
    private async void Home_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//MainPage");
    }

    // Event handler for the Finance button click event
    private async void Finance_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new FinancePage());
    }

    //METHODS FROM EDITPAGE.XAML.CS
    // NEED TO BE CHANGED FOR SPECIFIC IMPLEMENTATION OF FORM

    
    // Event handler for the Add New Stock button click event
    private void AddNewStock_Clicked(object sender, EventArgs e)
    {
        if (StockTypePicker.SelectedItem == null)
        {
            DisplayAlert("Error", "Please select a stock type", "OK");
            return;
        }
        if (string.IsNullOrWhiteSpace(aveCost.Text))
        {
            DisplayAlert("Error", "Please enter a value for cost", "OK");
            return;
        }
        if (!int.TryParse(aveCost.Text, out int cost))
        {
            DisplayAlert("Error", "Invalid value for cost", "OK");
            return;
        }
        if (!int.TryParse(AddQty.Text, out int qty))
        {
            DisplayAlert("Error", "Invalid value for Qty", "OK");
            return;
        }

        string selectedStockType = StockTypePicker.SelectedItem.ToString();

        int additionalField;
        if (!int.TryParse(AddProduce.Text, out additionalField))
        {
            DisplayAlert("Error", "Invalid value for additional field", "OK");
            return;
        }

        // Call the AddNewStock method from the ViewModel
        int result = _viewModel.AddNewStock(selectedStockType, cost, additionalField, qty);

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

    private void AveAddNewStock_Clicked(object sender, EventArgs e)
    {
        if (AveStockTypePicker.SelectedItem == null)
        {
            DisplayAlert("Error", "Please select a stock type", "OK");
            return;
        }
        if (!int.TryParse(AveAddQty.Text, out int qty))
        {
            DisplayAlert("Error", "Invalid value for Qty", "OK");
            return;
        }

        string selectedStockType = AveStockTypePicker.SelectedItem.ToString();


        // Call the AddNewStock method from the ViewModel
        int result = _viewModel.AddAveNewStock(selectedStockType, qty);

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

    // Event handler for the Stock Type Picker selection change event
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

    // Method to clear the add form fields
    private void ClearAddForm()
    {
        AveStockTypePicker.SelectedItem = null;
        AveAddQty.Text = string.Empty;
        StockTypePicker.SelectedItem = null;
        aveCost.Text = string.Empty;
        AddProduce.Text = string.Empty;
        AddQty.Text = string.Empty;
        AddProduceLabel.Text = "Produce";    // Reset column header to Produce
    }

    // Event handler for the Livestock button click event
    private async void Livestock_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new LivestockPage());
    }


    // Event handler to clear the add new stock fields
    private void ClearAddNewStock_Clicked(object sender, EventArgs e)
    {
        ClearAddForm();
    }
}