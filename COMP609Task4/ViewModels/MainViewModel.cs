
namespace COMP609Task4.ViewModels;
//use depenency injection (DI) to make this view model available throughout the app

public class MainViewModel
{
    public ObservableCollection<Stock> Stock { get; set; }
    public readonly Database _database;
    public MainViewModel()
    {
        _database = new();
        Stock = new();
        _database.ReadItems().ForEach(x => Stock.Add(x));
    }
    //public string GetGeneralStats()
    //{
    //    return $"Total staff: {Stock.Sum(x => x.NumStaff)}";
    //}
    //public string QueryByStoreType(string type)
    //{
    //    List<Stock> stk = Stock.Where(x => x.GetType().Name.Equals(type)).ToList();
    //    string results = $"{$"Number of {type}:",-30}{stk.Count}\n";
    //    results += $"{"Average number of staff:",-30}{stk.Average(x => x.NumStaff):F2}";
    //    return results;
    //}
}
