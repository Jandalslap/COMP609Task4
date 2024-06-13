using Windows.Storage.Search;

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

    //private readonly Database _database;
    //private readonly string _searchId;
    //private readonly EditViewModel _viewModel;


    // Constructor with a search ID parameter
    //public EditPage(string searchId) : this()
    //{
    //    _searchId = searchId;
    //    _viewModel.SearchById(searchId);
    //    PopulateFields(searchId); // Call PopulateFields after searching by ID
    //}

    // Event handler for the SearchById button click event
    //private void SearchById_Clicked(object sender, EventArgs e)
    //{
    //    var enteredId = IdSearchEntry.Text;
    //    if (!string.IsNullOrEmpty(enteredId))
    //    {
    //        _viewModel.SearchById(enteredId);
    //        PopulateFields(enteredId); // Call PopulateFields after searching by ID
    //    }
    //}

    // Method to populate fields with data from the database based on search ID
    //private void PopulateFields(string searchId)
    //{
    //    var stockToEdit = _database.GetItemById(searchId);
    //    if (stockToEdit != null)
    //    {
    //        // Bind stockToEdit to the UI
    //        BindingContext = stockToEdit;
    //        TypeLabel.Text = stockToEdit.Type;
    //        IdLabel.Text = stockToEdit.Id.ToString();
    //        CostEntry.Text = stockToEdit.Cost.ToString();
    //        WeightEntry.Text = stockToEdit.Weight.ToString();
    //        ColourLabel.Text = stockToEdit.Colour;

    //        if (stockToEdit is Cow cow)
    //        {
    //            ProduceLabel.Text = "Milk";
    //            ProduceEntry.Text = cow.Milk.ToString();
    //        }
    //        else if (stockToEdit is Sheep sheep)
    //        {
    //            ProduceLabel.Text = "Wool";
    //            ProduceEntry.Text = sheep.Wool.ToString();
    //        }
    //        else
    //        {
    //            ProduceLabel.Text = "Produce";
    //            ProduceEntry.Text = "Unknown";
    //        }
    //    }
    //    else
    //    {
    //        Console.WriteLine("Stock item not found with ID: " + searchId);
    //    }
    //}

    // Event handler for the Update button click event
    //private void Update_Clicked(object sender, EventArgs e)
    //{
    //    if (sender is Button button && button.CommandParameter is Stock updatedStock)
    //    {
    //        updatedStock.Cost = int.Parse(CostEntry.Text);
    //        updatedStock.Weight = int.Parse(WeightEntry.Text);

    //        if (updatedStock is Cow cow)
    //        {
    //            cow.Milk = int.Parse(ProduceEntry.Text);
    //        }
    //        else if (updatedStock is Sheep sheep)
    //        {
    //            sheep.Wool = int.Parse(ProduceEntry.Text);
    //        }

    //        int result = _database.UpdateItem(updatedStock);

    //        if (result > 0)
    //        {
    //            DisplayAlert("Success", "Stock updated successfully", "OK");
    //            ClearEditForm();
    //        }
    //        else
    //        {
    //            DisplayAlert("Error", "Failed to update stock", "OK");
    //        }
    //    }
    //}

    // Event handler for the Delete button click event
    //private async void Delete_Clicked(object sender, EventArgs e)
    //{
    //    if (sender is Button button && button.CommandParameter is Stock deletedStock)
    //    {
    //        // Display confirmation alert with cancel option
    //        bool deleteConfirmed = await DisplayAlert("Confirm Deletion", "Are you sure you want to delete this stock item?", "Yes", "Cancel");

    //        if (deleteConfirmed)
    //        {
    //            // User confirmed deletion
    //            _database.DeleteItem(deletedStock);
    //            await DisplayAlert("Success", "Stock deleted successfully", "OK");
    //            ClearEditForm();
    //        }
    //        // If cancel option was chosen, do nothing
    //    }
    //}

    // Event handler for the Add New Stock button click event
    private void AddNewStock_Clicked(object sender, EventArgs e)
    {
        if (StockTypePicker.SelectedItem == null)
        {
            DisplayAlert("Error", "Please select a stock type", "OK");
            return;
        }
        if (ColourPicker.SelectedItem == null)
        {
            DisplayAlert("Error", "Please select a colour", "OK");
            return;
        }
        if (string.IsNullOrWhiteSpace(AddCost.Text))
        {
            DisplayAlert("Error", "Please enter a value for cost", "OK");
            return;
        }
        if (string.IsNullOrWhiteSpace(AddWeight.Text))
        {
            DisplayAlert("Error", "Please enter a value for weight", "OK");
            return;
        }
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

        string selectedStockType = StockTypePicker.SelectedItem.ToString();
        string selectedColour = ColourPicker.SelectedItem.ToString();

        int additionalField;
        if (!int.TryParse(AddProduce.Text, out additionalField))
        {
            DisplayAlert("Error", "Invalid value for additional field", "OK");
            return;
        }

        // Call the AddNewStock method from the ViewModel
        int result = _viewModel.AddNewStock(selectedStockType, selectedColour, cost, weight, additionalField);

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
        StockTypePicker.SelectedItem = null;
        ColourPicker.SelectedItem = null;
        AddCost.Text = string.Empty;
        AddWeight.Text = string.Empty;
        AddProduce.Text = string.Empty;
    }

    // Event handler for the Livestock button click event
    private async void Livestock_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new LivestockPage());
    }

    // Method to clear the edit form fields
    //private void ClearEditForm()
    //{
    //    TypeLabel.Text = "Type";
    //    IdLabel.Text = "ID";
    //    ColourLabel.Text = "Colour";
    //    CostEntry.Text = string.Empty;
    //    WeightEntry.Text = string.Empty;
    //    ProduceLabel.Text = "Produce";
    //    ProduceEntry.Text = string.Empty;
    //    IdSearchEntry.Text = string.Empty;
    //}

    // Event handler to clear the search by ID fields
    //private void ClearSearchById_Clicked(object sender, EventArgs e)
    //{
    //    ClearEditForm();
    //}

    // Event handler to clear the add new stock fields
    private void ClearAddNewStock_Clicked(object sender, EventArgs e)
    {
        ClearAddForm();
    }
}