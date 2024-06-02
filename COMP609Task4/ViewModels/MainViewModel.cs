
namespace COMP609Task4.ViewModels;
//use depenency injection (DI) to make this view model available throughout the app

public class MainViewModel
{
    public ObservableCollection<Stock> Stock { get; set; }
    public readonly Database _database;
    public MainViewModel()
    {
        
    }

}
