using System.Globalization;
using Chirp.CSVDBService.Controllers;
using Chirp.CSVDBService.Models;
using CsvHelper;

namespace Chirp.CSVDBService.Tests.IntegrationTests;

public class IntegrationTestsChirpCsvDbService : IDisposable
{
    private readonly string _tempFile = Path.GetTempFileName(); // temporary csv db for testing

    public void Dispose()
    {
        if (File.Exists(_tempFile))
            File.Delete(_tempFile);
    }

    [Fact]
    private void StoreShouldStoreACheepInTheCsvdb()
    {
        // Arrange
        var controller = new CheepRepository<Cheep<string>>(_tempFile);
        var cheep = new Cheep<string>("Alice", "Lorem ipsum", 12345);
        using (var writer = new StreamWriter(_tempFile, append: true))
        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
        {
            csv.WriteHeader<Cheep<string>>();
            csv.NextRecord();
        }

        // Act
        controller.Store(cheep);

        // Assert
        List<Cheep<string>> records;
        using (var reader = new StreamReader(_tempFile))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            records = csv.GetRecords<Cheep<string>>().ToList();
        }

        Assert.Contains(cheep, records);
    }

    [Fact]
    private void ReadShouldReadCheepFromCsvdb()
    {
        // Arrange
        var cheep = new Cheep<string>("Alice", "Lorem ipsum", 12345);
        using (var writer = new StreamWriter(_tempFile, append: true))
        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
        {
            csv.WriteHeader<Cheep<string>>();
            csv.NextRecord();
            csv.WriteRecord(cheep);
            csv.NextRecord();
        }

        var controller = new CheepRepository<Cheep<string>>(_tempFile);

        // Act
        var result = controller.Read();

        // Assert
        Assert.Contains(cheep, result);
    }
}
