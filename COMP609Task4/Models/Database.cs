using System;
using SQLite;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace COMP609Task4.Models
{
    public class Database
    {
        private readonly SQLiteConnection _connection;

        // Constructor to initialize the database connection
        public Database()
        {
            string dbName = "FarmDataOriginal.db";
            string dbPath = Path.Combine(Current.AppDataDirectory, dbName);

            if (!File.Exists(dbPath))
            {
                // Open db file from the asset folder (Raw)
                using Stream stream = Current.OpenAppPackageFileAsync(dbName).Result;
                using MemoryStream memoryStream = new();
                stream.CopyTo(memoryStream);
                // Write db data to app directory
                File.WriteAllBytes(dbPath, memoryStream.ToArray());
            }

            _connection = new SQLiteConnection(dbPath);
            _connection.CreateTables<Cow, Sheep>(); // create if tables do not exist
        }

        // Method to read all items from the database
        public List<Stock> ReadItems()
        {
            var stock = new List<Stock>();
            var lst1 = _connection.Table<Cow>().ToList();
            stock.AddRange(lst1);
            var lst2 = _connection.Table<Sheep>().ToList();
            stock.AddRange(lst2);
            return stock;
        }

        public Stock GetItemById(string id)
        {
            if (!int.TryParse(id, out int itemId))
            {
                // USer input validation for the case where the input is not a valid integer
                return null; 
            }

            // Check if the ID is for a Cow or a Sheep
            var cow = _connection.Table<Cow>().FirstOrDefault(c => c.Id == itemId);
            if (cow != null)
            {
                return cow;
            }

            var sheep = _connection.Table<Sheep>().FirstOrDefault(s => s.Id == itemId);
            if (sheep != null)
            {
                return sheep;
            }

            return null; // Item not found
        }


        // Method to insert a new item into the database
        public int InsertItem(Stock item)
        {
            return _connection.Insert(item);
        }

        // Method to delete an item from the database
        public int DeleteItem(Stock item)
        {
            return _connection.Delete(item);
        }

        // Method to update an existing item in the database
        public int UpdateItem(Stock item)
        {
            Console.WriteLine($"Updating item: {item}");

            // Check if the item is of type Cow or Sheep and update accordingly
            if (item is Cow cow)
            {
                Console.WriteLine($"Updating Cow with ID: {cow.Id}, Milk: {cow.Milk}");
                return _connection.Update(cow);
            }
            else if (item is Sheep sheep)
            {
                Console.WriteLine($"Updating Sheep with ID: {sheep.Id}, Wool: {sheep.Wool}");
                return _connection.Update(sheep);
            }
            else
            {
                Console.WriteLine("Unknown stock type.");
                return 0;
            }
        }

        // Method to add a new item to the database
        public int AddItem(Stock item)
        {
            try
            {
                return _connection.Insert(item);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding item to database: {ex.Message}");
                return 0;
            }
        }
    }
}
