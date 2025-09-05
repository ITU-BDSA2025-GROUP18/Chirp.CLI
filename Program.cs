using System.Globalization;
using System.CommandLine;
using System.CommandLine.Parsing;
using Chirp.CLI;
using CsvHelper;

public class Program

{
    public static int Main(string[] args)
    {
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

        if (parseResult.GetResult(readCommand) != null) UserInterface.PrintCheeps();
        if (parseResult.GetResult(cheepCommand)?.GetValue(cheepArg) is { } message) Cheep(message);

        return 0;
    }

    //Work in progress. Skal kunne tilføje en besked til chirp_cli_db.csv med user og tidspunkt korrekt angivet
    private static void Cheep(string message)
    {
        var author = Environment.UserName;
        var utcTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        var record = new Cheep(author, message, utcTimestamp);

        using var writer = new StreamWriter("chirp_cli_db.csv", append: true);
        using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
        csv.WriteRecord(record);
        writer.WriteLine();
    }
}
