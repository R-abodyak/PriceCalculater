public interface IDisplayService
{
    public void Display(String msg,decimal value);
}
public class ConsoleDisplayService : IDisplayService
{
    private string formatString = "{0,0:" + ".00" + "}";
    public void Display(String msg, decimal value)
    {   if (value == 0) return;
        Console.Write(msg);
        Console.WriteLine(formatString, value);
    }
}
