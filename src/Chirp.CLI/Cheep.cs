using System.Globalization;

namespace Chirp.CLI;

public abstract record Cheep<T>(string Author, T Message, long Timestamp)
{
    public override string ToString()
    {
        var formattedTimeStamp = DateTimeOffset
            .FromUnixTimeSeconds(Timestamp)
            .LocalDateTime
            .ToString(CultureInfo.InvariantCulture);

        return $"{Author} @ {formattedTimeStamp}: {Message}";
    }
}