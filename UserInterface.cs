using System.Globalization;
using Chirp.CLI;
using CsvHelper;

public class UserInterface : Program
{
    //Function that gets all cheeps from the crip_cli_db.csv file
    public IEnumerable<Cheep> ReadCheaps() {
        using (var reader = new StreamReader("chirp_cli_db.csv"))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture)) 
        
            return csv.GetRecords<Cheep>().ToList();
    }

    //Function that writes out all cheeps in console
    public void PrintCheeps()
    {
        foreach (var cheep in ReadCheaps())
        {
            writeRecordToConsole(cheep);
        }
    }
    
    //Function that writes out a cheep in console.
    static void writeRecordToConsole(Cheep record)
    {
        var formattedTimeStamp = DateTimeOffset
            .FromUnixTimeSeconds(record.Timestamp)
            .LocalDateTime
            .ToString(CultureInfo.InvariantCulture);

        Console.WriteLine($"{record.Author} @ {formattedTimeStamp}: {record.Message}");  
    }
}