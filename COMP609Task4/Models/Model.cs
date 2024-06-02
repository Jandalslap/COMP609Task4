namespace COMP609Task4.Models
{
    public class Stock
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Type { get; set; } 
        public int Cost { get; set; }
        public int Weight { get; set; }
        public string Colour { get; set; }
        public string Milk { get; set; } = "-";  // Default to "-" if not applicable
        public string Wool { get; set; } = "-";  // Default to "-" if not applicable

        public Stock()
        {
            Type = GetType().Name; // Set Type property to the type name of the object
        }
        public override string ToString()
        { 
            return $"{Type,-15} {Id,-5} {Cost,-5} {Weight,-5} {Colour,-10}";
        }
    }

    // Annotation to match to database table - not required if the table name is the same
    [Table("Sheep")]

    // Subclass of Stock
    public class Sheep : Stock
    {
        public new int? Wool { get; set; }
        public override string ToString()
        {
            return base.ToString() + $"{Wool}";
        }
    }

    [Table("Cow")]
    public class Cow : Stock
    {
        public new int? Milk { get; set; }
        public override string ToString()
        {
            return base.ToString() + $"{Milk}";

        }
    }
}
