public interface IDisplayService
{
    public void Display(String msg,decimal value, String currency);
}
public class ConsoleDisplayService : IDisplayService
{
    private string formatString = "{0,0:" + ".00" + "}";
    public void Display(String msg, decimal value, String currency)
    {   if (value == 0) return;
        Console.Write(msg);
        Console.WriteLine(formatString, value);
        Console.Write(formatString, value);
        Console.WriteLine("  " + currency);
    }
}
