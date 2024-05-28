namespace COMP609Task4.Models;

public class Database
{
    private readonly SQLiteConnection _connection;
    public Database()
    {
        string dbName = "FarmDataOriginal.db";
        string dbPath = Path.Combine(Current.AppDataDirectory, dbName);

        if (!File.Exists(dbPath))
        {
            //open db file from the asset folder (Raw)
            using Stream stream = Current.OpenAppPackageFileAsync(dbName).Result;
            using MemoryStream memoryStream = new();
            stream.CopyTo(memoryStream);
            //write db data to app directory
            File.WriteAllBytes(dbPath, memoryStream.ToArray());
        }

        _connection = new SQLiteConnection(dbPath);
        _connection.CreateTables<Cow, Sheep>(); // create if tables do not exist
    }
    public List<Stock> ReadItems()
    {
        var stock = new List<Stock>();
        var lst1 = _connection.Table<Cow>().ToList();
        stock.AddRange(lst1);
        var lst2 = _connection.Table<Sheep>().ToList();
        stock.AddRange(lst2);
        return stock;
    }
    public int InsertItem(Stock item) // uses true type of item to determine which subclass table to enter or delete from
    {
        return _connection.Insert(item);
    }

    public int DeleteItem(Stock item)
    {
        return _connection.Delete(item);
    }
}
