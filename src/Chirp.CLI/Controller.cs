using System.CommandLine;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Chirp.CLI;

public class Controller
{
    private readonly HttpClient _client = new();

    public int Run(string[] args)
    {
        // ---- HTTP ---- //
        var baseURL = "http://localhost:5012";
        _client.DefaultRequestHeaders.Accept.Clear();
        _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        _client.BaseAddress = new Uri(baseURL);

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

            UserInterface<Cheep<string>>.PrintCheeps(ReadCheeps(readAmount));
        }

        if (parseResult.GetResult(cheepCommand)?.GetValue(cheepArg) is { } message)
        {
            var author = Environment.UserName;
            var utcTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            //new Cheep<string>(author, message, utcTimestamp);
        }

        return 0;
    }

    async IAsyncEnumerable<Cheep<string>> ReadCheeps(int? limit = null)
    {
        var cheeps = await _client.GetFromJsonAsync<Cheep<string>>($"cheeps?limit={limit}");
        yield return cheeps;
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