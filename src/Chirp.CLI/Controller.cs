using System.CommandLine;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Chirp.CLI;

public class Controller
{
    private readonly HttpClient _client;
    private string _baseURL;

    public Controller()
    {
        // ---- HTTP ---- //
        _client = new HttpClient();
        _baseURL = "http://localhost:5135";
        _client.DefaultRequestHeaders.Accept.Clear();
        _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        _client.BaseAddress = new Uri(_baseURL);
    }

    public async Task<int> Run(string[] args)
    {
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
            await ReadCheepsFromCSVDBService(readAmount);
        }

        if (parseResult.GetResult(cheepCommand)?.GetValue(cheepArg) is { } message)
        {
            var author = Environment.UserName;
            var utcTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            //new Cheep<string>(author, message, utcTimestamp);
        }

        return 0;
    }

    async Task ReadCheepsFromCSVDBService(int? limit = null)
    {
        var url = limit.HasValue
            ? $"cheeps?limit={limit.Value}"
            : "cheeps";
        var cheeps = await _client.GetFromJsonAsync<List<Cheep<string>>>(url);
        if (cheeps != null)
        {
            await UserInterface<Cheep<string>>.PrintCheeps(cheeps);
        }
        else
        {
            throw new Exception("No cheeps found");
        }
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