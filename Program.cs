using System.Globalization;
using Chirp.CLI;
using CsvHelper;

// Læser alle beskeder fra chirp_cli_db.csv-filen. Bemærk dag/måned er omvendt af Eduards på GitHub...
void read()
{
    using (var reader = new StreamReader("chirp_cli_db.csv"))
    using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
    {
        var records = csv.GetRecords<Cheep>();
        records.ToList().ForEach(record => writeRecordToConsole(record));
    }
}

void writeRecordToConsole(Cheep record)
{
    var formattedTimeStamp = DateTimeOffset
        .FromUnixTimeSeconds(record.Timestamp)
        .LocalDateTime
        .ToString(CultureInfo.InvariantCulture);
    Console.WriteLine(record.Author + " @ " + formattedTimeStamp + " " + record.Message);
    Thread.Sleep(1000);
}

//Work in progress. Skal kunne tilføje en besked til chirp_cli_db.csv med user og tidspunkt korrekt angivet
void cheep()
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
    
    using (var writer = new StreamWriter("chirp_cli_db.csv"))
    using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
    {
        csv.WriteRecord(record);
    }
}

//Koden nedenfor er basically vores main
if (args.Length < 1) Console.WriteLine("Invalid argument(s)");
if (args[0] == "read") read();
else if (args[0] == "cheep") cheep();