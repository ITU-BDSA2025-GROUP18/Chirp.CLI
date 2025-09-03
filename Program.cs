using System;
using System.IO;
using System.Runtime.Serialization;
using System.Text;

public class Program
{
    // Læser alle beskeder fra chirp_cli_db.csv-filen. Bemærk dag/måned er omvendt af Eduards på GitHub...
    void read()
    {
        string filepath = "chirp_cli_db.csv";

        StreamReader reader = new StreamReader(filepath);

        reader.ReadLine(); //Skipper første linje i filen: "Author,Message,Timestamp", da denne ikke er en besked

        while (!reader.EndOfStream)
        {
            string[] line = reader.ReadLine()!.Split(",");

            string author = line[0];

            StringBuilder sb = new StringBuilder();
            for (int i = 1; i < line.Length - 1; i++)
            {
                sb.Append(line[i]);
                if (i < line.Length - 2) sb.Append(",");
            }

            sb.Replace("\"", ""); //Formatterer beskeden rigtigt
            string message = sb.ToString();

            long unixTimestamp = long.Parse(line[line.Length - 1]);
            string timestamp = DateTimeOffset.FromUnixTimeSeconds(unixTimestamp).LocalDateTime.ToString();

            Console.WriteLine(author + " @ " + timestamp + ": " + message);

            Thread.Sleep(1000);
        }
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
        string utcTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();

        string line = author + ",\"" + message + "\"," + utcTimestamp;

        StreamWriter sw = File.AppendText("chirp_cli_db.csv");
        sw.WriteLine(line);
        sw.Close();
    }
}