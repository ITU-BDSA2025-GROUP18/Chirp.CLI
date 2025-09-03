using System.Globalization;
using Chirp.CLI;
using CsvHelper;

public class UserInterface : Program
{
    //Function that writes out all cheeps from the crip_cli_db.csv file
    public IEnumerable<Cheep> ReadCheaps() {
        using (var reader = new StreamReader("chirp_cli_db.csv"))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture)) 
        
            return csv.GetRecords<Cheep>().ToList();
    }

    public void PrintCheeps()
    {
        foreach (var cheep in ReadCheaps())
        {
            writeRecordToConsole(cheep);
        }
    }

    static void writeRecordToConsole(Cheep record)
    {
        var formattedTimeStamp = DateTimeOffset
            .FromUnixTimeSeconds(record.Timestamp)
            .LocalDateTime
            .ToString(CultureInfo.InvariantCulture);

        Console.WriteLine($"{record.Author} @ {formattedTimeStamp}: {record.Message}");  
    }
}