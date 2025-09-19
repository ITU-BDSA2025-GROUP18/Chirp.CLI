namespace Chirp.CLI;

public class Program
{
    public static int Main(string[] args)
    {
        var controller = new Controller();
        return controller.Run(args);
    }
}