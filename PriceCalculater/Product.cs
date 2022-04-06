public class Product
{
    public String Name { get; set; } = "undefined name";
    public long UPC { get; set; }
    public decimal Price { get; set; }
    public Iso_3 Currency { get; set; } = Iso_3.USD
}
public enum Iso_3 { USD ,GBP,JPY};
