namespace COMP609Task4.Models
{
    #region Stock Class
    public class Stock
    {
        #region Properties
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Type { get; set; } 
        public int Cost { get; set; }
        public int Weight { get; set; }
        public string Colour { get; set; }
        public string Milk { get; set; } = "-";  // Default to "-" if not applicable
        public string Wool { get; set; } = "-";  // Default to "-" if not applicable

        public decimal TaxCalculation { get; set; }
        public decimal IncomeCalculation { get; set; }
        #endregion
        #region Constructor
        // Default Constructor
        public Stock()
        {
            Type = GetType().Name; // Set Type property to the type name of the object
        }
        #endregion
        #region Methods
        // Override of ToString to provide a formatted string representation of the stock item
        public override string ToString()
        { 
            return $"{Type,-15} {Id,-5} {Cost,-5} {Weight,-5} {Colour,-10}";
        }
        #endregion
    }
    #endregion
    #region Sheep Class
    // Annotation to specify the corresponding database table name
    [Table("Sheep")]

    // Sheep class inheriting from Stock
    public class Sheep : Stock
    {
        #region Properties
        // Wool production specific to Sheep, nullable to indicate possible absence of value
        public new int? Wool { get; set; }
        #endregion
        #region Methods
        // Override of ToString to include wool information for sheep
        public override string ToString()
        {
            // Append wool information to the base string representation
            return base.ToString() + $"{Wool}";
        }
        #endregion
    }
    #endregion
    #region Cow Class
    // Annotation to specify the corresponding database table name
    [Table("Cow")]
    // Cow class inheriting from Stock
    public class Cow : Stock
    {
        #region Properties
        // Milk production specific to Cow, nullable to indicate possible absence of value
        public new int? Milk { get; set; }
        #endregion
        #region Methods
        // Override of ToString to include milk information for cows
        public override string ToString()
        {
            // Append milk information to the base string representation
            return base.ToString() + $"{Milk}";
        }
        #endregion
    }
    #endregion
}
