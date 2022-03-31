public interface IDisplayService
{
    public void Display(decimal value);
}
public class ConsoleDisplayService : IDisplayService
{
    private string formatString = "{0,0:" + ".00" + "}";
  
    public void Display(decimal value)
    {
        Console.WriteLine(formatString, value);
    }
}


