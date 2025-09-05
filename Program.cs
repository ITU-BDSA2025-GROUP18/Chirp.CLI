using System.Globalization;
using System.CommandLine;
using System.CommandLine.Parsing;
using Chirp.CLI;
using CsvHelper;

public class Program

{
    public static int Main(string[] args)
    {
        var UI = new UserInterface();

        RootCommand rootCommand = new RootCommand("Chirp command line interface");

        Command readCommand = new Command("read", "Read messages in the database");
        rootCommand.Add(readCommand);

        Command cheepCommand = new Command("cheep", "Send a message to the database");
        Argument<string> cheepArg = new Argument<string>("Message that'll be sent to the database");
        cheepCommand.Add(cheepArg);
        rootCommand.Add(cheepCommand);

        ParseResult parseResult = rootCommand.Parse(args);
        if (parseResult.Errors.Count > 0)
        {
            foreach (ParseError parseError in parseResult.Errors)
            {
                Console.Error.WriteLine(parseError.Message);
            }
            return 1;
        }

        if (parseResult.GetResult(readCommand) != null) UI.PrintCheeps();
        if (parseResult.GetResult(cheepCommand)?.GetValue<string>(cheepArg) is string message) cheep(message);

        return 0;
    }

    //Tilføjer en besked til chirp_cli_db.csv, inkl. user og tidspunkt
    static void cheep(string message)
    {
        string author = Environment.UserName;
        long utcTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        var record = new Cheep(author, message, utcTimestamp);

        using (var writer = new StreamWriter("chirp_cli_db.csv", append: true))
        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
        {
            csv.WriteRecord(record);
            writer.WriteLine();
        }
    }
}
