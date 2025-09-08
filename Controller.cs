using System.CommandLine;

namespace Chirp.CLI;

using SimpleDB;

public class Controller
{
    private readonly CSVDatabase<Cheep<string>> _database;

    public Controller(CSVDatabase<Cheep<string>> database)
    {
        _database = database;
    }

    public int Run(string[] args)
    {
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

        if (parseResult.GetResult(readCommand) != null) UserInterface.PrintCheeps(_database.Read(20));
        if (parseResult.GetResult(cheepCommand)?.GetValue(cheepArg) is { } message)
            _database.Store(new Cheep<string>(author, message, utcTimestamp));

        return 0;
    }
}