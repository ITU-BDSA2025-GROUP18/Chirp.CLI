using System.Globalization;
using CsvHelper;

namespace Chirp.CSVDBService;

public class CheepRepository
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
}