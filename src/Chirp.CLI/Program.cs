using Chirp.CLI;
using SimpleDB;

public class Program
{
    public static int Main(string[] args)
    {
        var dataPath = "chirp_cli_db.csv";
        var database = new CSVDatabase<Cheep<string>>(dataPath);
        var controller = new Controller(database);
        return controller.Run(args);
    }
}