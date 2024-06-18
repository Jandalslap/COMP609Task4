namespace COMP609Task4.Pages;

public partial class MainPage : ContentPage
{
    #region Constructor
    // Constructor for MainPage
    public MainPage(MainViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
    #endregion
    #region Navigation Methods
    // Event handler for the Livestock button click event
    private async void Livestock_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new LivestockPage());
    }

    // Event handler for the Finance button click event
    private async void Finance_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new FinancePage());
    }
    #endregion
}