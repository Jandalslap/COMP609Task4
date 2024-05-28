namespace COMP609Task4.Pages;

public partial class MainPage : ContentPage
{
    public MainPage(MainViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    private async void Livestock_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new LivestockPage());
    }

    private async void Finance_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new FinancePage());
    }
}