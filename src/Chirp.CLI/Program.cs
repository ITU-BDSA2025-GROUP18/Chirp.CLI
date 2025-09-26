namespace Chirp.CLI;

public class Program
{
    public static async Task<int> Main(string[] args)
    {
        var controller = new Controller("http://localhost:8080");

        return await controller.Run(args);
    }
}
