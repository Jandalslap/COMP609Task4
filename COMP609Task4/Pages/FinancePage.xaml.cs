namespace COMP609Task4.Pages;

public partial class FinancePage : ContentPage
{
    public FinancePage()
    {
        InitializeComponent();
    }

    private async void Back_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync(); // Go back to the previous page
    }

    private async void Forecast_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ForecastPage()); // Navigate to the ForecastPage
    }
    private async void Home_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//MainPage");
    }
}