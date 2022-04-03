public interface IDisplayService
{
    public void Display(decimal value);
}
public class ConsoleDisplayService : IDisplayService
{
    private string formatString = "{0,0:" + ".00" + "}";
    public void Display(decimal value)
    {   if (value == 0) return;
        Console.WriteLine(formatString, value);
    }
}
