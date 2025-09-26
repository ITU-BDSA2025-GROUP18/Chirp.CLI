using System.Globalization;
using Chirp.CSVDBService.Models;
using CsvHelper;

namespace Chirp.CSVDBService.Controllers;

public class CheepRepository<T>
{
    private string _path;
    private StreamWriter _writer;
    private CsvWriter _csv;

    public CheepRepository()
    {
        var home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        _path = Path.Combine(home, "chirp_cli_db.csv");


        var dir = Path.GetDirectoryName(_path);
        if (!Directory.Exists(dir))
            Directory.CreateDirectory(dir);

        if (!File.Exists(_path))
        {
            using var writer = new StreamWriter(_path);
            using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
            csv.WriteHeader<T>();
            csv.NextRecord();
        }
    }

    public IEnumerable<T> Read(int? limit = null)
    {
        using var reader = new StreamReader(_path);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

        var records = csv.GetRecords<T>().ToList();
        var n = records.Count;

        if (limit == null) return records;
        if (limit < 0) throw new ArgumentException("limit must be positive");
        if (limit > n) limit = n;

        return records.GetRange(0, limit.Value);
    }

    public void Store(T record)
    {
        using var writer = new StreamWriter(_path, append: true);
        using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
        csv.WriteRecord(record);
        writer.WriteLine();
    }
}
