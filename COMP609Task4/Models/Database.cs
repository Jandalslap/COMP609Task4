using System;
using SQLite;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace COMP609Task4.Models
{
    public class Database
    {
        #region Private Members
        // SQLite connection instance for database interactions
        private readonly SQLiteConnection _connection;
        #endregion
        #region Constructor
        // Constructor to initialize the database connection
        public Database()
        {
            string dbName = "FarmDataOriginal.db";
            string dbPath = Path.Combine(Current.AppDataDirectory, dbName);

            if (!File.Exists(dbPath))
            {
                // Open db file from the asset folder (Raw)
                using Stream stream = Current.OpenAppPackageFileAsync(dbName).Result;
                // Create a memory stream to hold the database data
                using MemoryStream memoryStream = new();
                // Copy the database data to the memory stream
                stream.CopyTo(memoryStream);
                // Write db data to app directory
                File.WriteAllBytes(dbPath, memoryStream.ToArray());
            }
            // Initialize the SQLite connection with the database path
            _connection = new SQLiteConnection(dbPath);
            // Ensure tables for Cow and Sheep exist in the database
            _connection.CreateTables<Cow, Sheep>(); // create if tables do not exist
        }
        #endregion
        #region Read Operations
        // Method to read all items (Cow and Sheep) from the database
        public List<Stock> ReadItems()
        {
            // Create a list to hold all stock items
            var stock = new List<Stock>();
            // Retrieve all Cow records from the database and add to the stock list
            var lst1 = _connection.Table<Cow>().ToList();
            stock.AddRange(lst1);
            // Retrieve all Sheep records from the database and add to the stock list
            var lst2 = _connection.Table<Sheep>().ToList();
            stock.AddRange(lst2);
            return stock;
        }
        // Method to retrieve a specific item by its ID
        public Stock GetItemById(string id)
        {
            // Validate the ID to ensure it's a valid integer
            if (!int.TryParse(id, out int itemId))
            {
                // Return null if the input is not a valid integer
                return null; 
            }

            // Check if the ID is for a Cow
            var cow = _connection.Table<Cow>().FirstOrDefault(c => c.Id == itemId);
            if (cow != null)
            {
                return cow;
            }
            // Check if the ID is for a Sheep
            var sheep = _connection.Table<Sheep>().FirstOrDefault(s => s.Id == itemId);
            if (sheep != null)
            {
                return sheep;
            }
            // Return null if the item is not found
            return null;
        }
        #endregion
        #region Write Operations
        // Method to insert a new item (Cow or Sheep) into the database
        public int InsertItem(Stock item)
        {
            // Insert the item into the database and return the result
            return _connection.Insert(item);
        }

        // Method to delete an item from the database
        public int DeleteItem(Stock item)
        {
            // Delete the item from the database and return the result
            return _connection.Delete(item);
        }

        // Method to update an existing item in the database
        public int UpdateItem(Stock item)
        {
            Console.WriteLine($"Updating item: {item}");

            // Check if the item is of type Cow
            if (item is Cow cow)
            {
                Console.WriteLine($"Updating Cow with ID: {cow.Id}, Milk: {cow.Milk}");
                // Update the Cow item in the database and return the result
                return _connection.Update(cow);
            }
            // Check if the item is of type Sheep
            else if (item is Sheep sheep)
            {
                Console.WriteLine($"Updating Sheep with ID: {sheep.Id}, Wool: {sheep.Wool}");
                // Update the Sheep item in the database and return the result
                return _connection.Update(sheep);
            }
            else
            {
                // Log unknown stock type and return 0 for failure
                Console.WriteLine("Unknown stock type.");
                return 0;
            }
        }

        // Method to add a new item to the database
        public int AddItem(Stock item)
        {
            try
            {
                // Attempt to insert the item into the database and return the result
                return _connection.Insert(item);
            }
            catch (Exception ex)
            {
                // Log the exception message and return 0 for failure
                Console.WriteLine($"Error adding item to database: {ex.Message}");
                return 0;
            }
        }
        #endregion
    }
}
