namespace COMP609Task4.Pages;

public partial class ForecastPage : ContentPage
{
    public ForecastPage()
    {
        InitializeComponent();
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
}