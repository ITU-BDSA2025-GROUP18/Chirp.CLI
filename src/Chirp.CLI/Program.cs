namespace Chirp.CLI;

public class Program
{
    public static async Task<int> Main(string[] args)
    {
        var controller = new Controller();
        return await controller.Run(args); // Run is async Task<int>
    }
}