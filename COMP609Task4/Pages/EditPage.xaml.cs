namespace COMP609Task4.Pages;

public partial class EditPage : ContentPage
{
    public EditPage()
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
}