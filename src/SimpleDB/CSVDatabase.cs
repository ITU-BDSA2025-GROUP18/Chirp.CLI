using System.Globalization;
using CsvHelper;

namespace SimpleDB;

public sealed class CSVDatabase<T>(string path) : IDatabaseRepository<T>
{
    public IEnumerable<T> Read(int? limit = null)
    {
        using var reader = new StreamReader(path);
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
        using var writer = new StreamWriter(path, append: true);
        using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
        csv.WriteRecord(record);
        writer.WriteLine();
    }
}