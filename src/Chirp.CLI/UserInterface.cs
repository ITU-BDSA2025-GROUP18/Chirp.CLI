using System.Globalization;

namespace Chirp.CLI;

public abstract class UserInterface : Program
{
    // Function that writes out all cheeps in console
    public static void PrintCheeps(IEnumerable<Cheep<string>> cheeps)
    {
        foreach (var cheep in cheeps)
        {
            WriteRecordToConsole(cheep);
        }
    }

    // Function that writes out a cheep in console.
    private static void WriteRecordToConsole(Cheep<string> record)
    {
        var formattedTimeStamp = DateTimeOffset
            .FromUnixTimeSeconds(record.Timestamp)
            .LocalDateTime
            .ToString(CultureInfo.InvariantCulture);

        Console.WriteLine($"{record.Author} @ {formattedTimeStamp}: {record.Message}");
    }
}