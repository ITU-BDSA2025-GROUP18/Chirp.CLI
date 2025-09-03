using System.Globalization;
using Chirp.CLI;
using CsvHelper;
using CsvHelper.Configuration;

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
            Console.WriteLine(cheep);
        }
    }
}