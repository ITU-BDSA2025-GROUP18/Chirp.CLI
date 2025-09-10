using System.Globalization;
using CsvHelper;

namespace SimpleDB;

public sealed class CSVDatabase<T>(string path) : IDatabaseRepository<T> //Sealed modifier prevents subclasses
{
    public IEnumerable<T> Read(int? limit = null) //For at gøre den generic (Fremgået også på billedet
    {
        using var reader = new StreamReader(path);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

        var records = csv.GetRecords<T>().ToList();
        var n = records.Count;

        if (limit == null) return records;
        if (limit < 0) throw new ArgumentException("limit must be positive");
        if (limit > n) throw new ArgumentException($"limit must be less than DB-entry size ({n})");
        
        return records.GetRange(0, limit.Value);

        /*if (limit is int temp)
            return csv.GetRecords<T>().ToList().GetRange(0, temp);
        else
            return csv.GetRecords<T>().ToList();*/
    }

    public void Store(T record) //For at gøre den generic (Fremgår også på billedet)
    {
        using var writer = new StreamWriter(path, append: true);
        using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
        csv.WriteRecord(record);
        writer.WriteLine();
    }
}