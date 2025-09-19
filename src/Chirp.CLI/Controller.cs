using System.CommandLine;

namespace Chirp.CLI;

public class Controller
{
    public int Run(string[] args)
    {
        // ---- HTTP ---- //


        // ---- COMMANDS ---- //
        var rootCommand = new RootCommand("Chirp command line interface");
        var readCommand = new Command("read", "Read messages in the database");
        var readArgument = new Argument<int?>("readAmount");
        readCommand.Arguments.Add(readArgument);
        rootCommand.Subcommands.Add(readCommand);

        var cheepCommand = new Command("cheep", "Send a message to the database");
        var cheepArg = new Argument<string>("Message that'll be sent to the database");
        cheepCommand.Add(cheepArg);
        rootCommand.Add(cheepCommand);

        var parseResult = rootCommand.Parse(args);
        HandleParseErrors(parseResult);

        if (parseResult.GetResult(readCommand) != null)
        {
            var readAmount = parseResult.GetValue<int?>("readAmount");
            // READ CHEEPS
            UserInterface.PrintCheeps
        }

        if (parseResult.GetResult(cheepCommand)?.GetValue(cheepArg) is { } message)
        {
            var author = Environment.UserName;
            var utcTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            //new Cheep<string>(author, message, utcTimestamp);
        }

        return 0;
    }

    private static void HandleParseErrors(ParseResult parseResult)
    {
        if (parseResult.Errors.Count == 0) return;

        foreach (var parseError in parseResult.Errors)
        {
            Console.Error.WriteLine(parseError.Message);
        }

        Environment.Exit(1);
    }
}