using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using COMP609Task4.Models;

namespace COMP609Task4.ViewModels
{
    // ViewModel for managing editing operations
    public class EditViewModel : INotifyPropertyChanged
    {
        // Database instance for accessing data
        private readonly Database _database;

        // Collection to hold search results
        private ObservableCollection<Stock> _searchResults;

        // Property to access the database instance
        public Database Database => _database;

        // Collection property to hold search results
        public ObservableCollection<Stock> SearchResults
        {
            get => _searchResults;
            set
            {
                _searchResults = value;
                OnPropertyChanged(nameof(SearchResults)); // Notify property changed
            }
        }

        // Constructor
        public EditViewModel()
        {
            _database = new Database(); // Initialize database instance
            SearchResults = new ObservableCollection<Stock>(); // Initialize search results collection
        }

        // Method to search for items by ID
        public void SearchById(string id)
        {
            // Query the database for items with matching ID
            var result = _database.ReadItems().Where(item => item.Id.ToString() == id).ToList();
            SearchResults = new ObservableCollection<Stock>(result); // Update search results collection
        }

        // INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); // Notify property changed
        }
    }
}
