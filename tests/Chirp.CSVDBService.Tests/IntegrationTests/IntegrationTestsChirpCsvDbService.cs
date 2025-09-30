using System.Globalization;
using Chirp.CSVDBService.Controllers;
using Chirp.CSVDBService.Models;
using CsvHelper;

namespace Chirp.CSVDBService.Tests.IntegrationTests;

public class IntegrationTestsChirpCsvDbService
{
    [Fact]
    private void StoreShouldStoreACheepInTheCsvdb()
    {
        // Arrange
        var controller = new CheepRepository<Cheep<string>>();
        var cheep = new Cheep<string>("Alice", "Lorem ipsum", 12345);

        var home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        var path = Path.Combine(home, "chirp_cli_db.csv");

        // Act
        controller.Store(cheep);
        using var reader = new StreamReader(path);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        var records = csv.GetRecords<Cheep<string>>().ToList();

        // Assert
        Assert.Contains(cheep, records);
    }

    [Fact]
    private void ReadShouldReadCheepFromCsvdb()
    {
        // Arrange
        var controller = new CheepRepository<Cheep<string>>();
        var cheep = new Cheep<string>("Alice", "Lorem ipsum", 12345);

        using var writer = new StreamWriter(_path, append: true);
        using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
        csv.WriteRecord(record);
        writer.WriteLine();
    }
}
