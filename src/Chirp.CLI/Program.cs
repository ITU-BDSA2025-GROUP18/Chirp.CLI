using SimpleDB;

namespace Chirp.CLI;

public class Program
{
    public static int Main(string[] args)
    {
        const string dataPath = "chirp_cli_db.csv";
        var database = new CSVDatabase<Cheep<string>>(dataPath);
        var controller = new Controller(database);
        return controller.Run(args);
    }
}