namespace COMP609Task4.Models;

public class Stock
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string Type { get; set; } // Add Type property
    public int Cost { get; set; }
    public int Weight { get; set; }
    public string Colour { get; set; }
    public string Milk { get; set; } = "-";  // Default to "-" if not applicable
    public string Wool { get; set; } = "-";  // Default to "-" if not applicable

    public Stock()
    {
        Type = GetType().Name; // Set Type property to the type name of the object
    }
    public override string ToString() // base ToString()
    { // first name is for business type name
        return $"{Type,-15} {Id,-5} {Cost,-5} {Weight,-5} {Colour,-10}";
    }
    public string MilkOrWool
    {
        get
        {
            if (this is Cow cow)
            {
                return $"Milk: {cow.Milk}";
            }
            else if (this is Sheep sheep)
            {
                return $"Wool: {sheep.Wool}";
            }
            return string.Empty;
        }
    }
}


// annotaion to match to database table - not required if the table name is the same
[Table("Sheep")]

// subclass of Stock
public class Sheep : Stock
{
    public int Wool { get; set; }
    public override string ToString()
    {
        return base.ToString() + $"{Wool}";
    }
}

[Table("Cow")]
public class Cow : Stock
{
    public int Milk { get; set; }
    public override string ToString()
    {
        return base.ToString() + $"{Milk}";

    }
}
