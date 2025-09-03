using System.Globalization;
using Chirp.CLI;
using CsvHelper;

public class Program
{
    public static void Main(string[] args)
    {
        if (args.Length < 1) Console.WriteLine("Invalid argument(s)");
        if (args[0] == "cheep") cheep(args);
    }

//Work in progress. Skal kunne tilføje en besked til chirp_cli_db.csv med user og tidspunkt korrekt angivet
    static void cheep(string[] args)
    {
        if (args.Length < 2)
        {
            Console.WriteLine("Wrong syntax -- \"cheep\"-argument needs to be followed by a message");
            return;
        }

        string author = Environment.UserName;
        string message = args[1];
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