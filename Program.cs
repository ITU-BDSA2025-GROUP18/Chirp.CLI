using System.Globalization;
using System.CommandLine;
using CsvHelper;
using SimpleDB;

public class Program
{
    public static int Main(string[] args)
    {
        var dataPath = "chirp_cli_db.csv";
        var database = new CSVDatabase<Cheep<string>>(dataPath);

        var author = Environment.UserName;
        var utcTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        var rootCommand = new RootCommand("Chirp command line interface");
        var readCommand = new Command("read", "Read messages in the database");
        rootCommand.Add(readCommand);

        var cheepCommand = new Command("cheep", "Send a message to the database");
        var cheepArg = new Argument<string>("Message that'll be sent to the database");
        cheepCommand.Add(cheepArg);
        rootCommand.Add(cheepCommand);

        var parseResult = rootCommand.Parse(args);
        if (parseResult.Errors.Count > 0)
        {
            foreach (var parseError in parseResult.Errors)
            {
                Console.Error.WriteLine(parseError.Message);
            }

            return 1;
        }

        if (parseResult.GetResult(readCommand) != null) UserInterface.PrintCheeps(database.Read(20));
        if (parseResult.GetResult(cheepCommand)?.GetValue(cheepArg) is { } message)
            database.Store(new Cheep<string>(author, message, utcTimestamp));

        return 0;
    }
}