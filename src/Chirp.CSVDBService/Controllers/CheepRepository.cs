using System.Globalization;
using Chirp.CSVDBService.Models;
using CsvHelper;

namespace Chirp.CSVDBService.Controllers;

public class CheepRepository<T>
{
    private string _path = "Data/chirp_cli_db.csv";
    private StreamWriter _writer;
    private CsvWriter _csv;

    public CheepRepository()
    {
        if (!File.Exists(_path))
        {
            using var writer = new StreamWriter(_path);
            using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
            csv.WriteHeader<T>();
        }

        // Open the main writer for appending
        _writer = new StreamWriter(_path, append: true);
        _csv = new CsvWriter(_writer, CultureInfo.InvariantCulture);
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