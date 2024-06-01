using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using COMP609Task4.Models;

namespace COMP609Task4.ViewModels
{
    public class EditViewModel : INotifyPropertyChanged
    {
        private readonly Database _database;
        private ObservableCollection<Stock> _searchResults;

        public Database Database => _database; // Property to access the database instance

        public ObservableCollection<Stock> SearchResults
        {
            get => _searchResults;
            set
            {
                _searchResults = value;
                OnPropertyChanged(nameof(SearchResults));
            }
        }

        public EditViewModel()
        {
            _database = new Database();
            SearchResults = new ObservableCollection<Stock>();
        }

        public void SearchById(string id)
        {
            var result = _database.ReadItems().Where(item => item.Id.ToString() == id).ToList();
            SearchResults = new ObservableCollection<Stock>(result);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
