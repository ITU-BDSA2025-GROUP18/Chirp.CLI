/*using Chirp.CLI.Data;
using SimpleDB;

namespace Chirp.CLI;

public class Program
{
    public static int Main(string[] args)
    {
        var csvPath = DataLoader.GetCsvPath();
        var database = new CSVDatabase<Cheep<string>>(csvPath);
        var controller = new Controller(database);
        return controller.Run(args);
    }
}*/