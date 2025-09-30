using System.Globalization;
using System.Net.Http.Json;
using Chirp.CSVDBService.Controllers;
using Chirp.CSVDBService.Models;
using CsvHelper;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Builder;

namespace Chirp.CSVDBService.Tests.IntegrationTests;

public class IntegrationTestsChirpCsvDbService : IDisposable
{
    private readonly string _tempFile = Path.GetTempFileName(); // temporary csv db for testing

    public void Dispose()
    {
        if (File.Exists(_tempFile))
            File.Delete(_tempFile);
    }

    private void ArrangeCsv()
    {
        using (var writer = new StreamWriter(_tempFile, append: true))
        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
        {
            csv.WriteHeader<Cheep<string>>();
            csv.NextRecord();
        }
    }

    [Fact]
    private void StoreShouldStoreACheepInTheCsvdb()
    {
        // Arrange
        var controller = new CheepRepository<Cheep<string>>(_tempFile);
        var cheep = new Cheep<string>("Alice", "Lorem ipsum", 12345);
        ArrangeCsv();

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

    [Fact]
    private void Cheep_Then_Read_ShouldReturnMessage()
    {
        // Arrange
        var cheep = new Cheep<string>("Alice", "Lorem ipsum", 12345);
        var controller = new CheepRepository<Cheep<string>>(_tempFile);
        ArrangeCsv();

        // Act
        controller.Store(cheep);
        var records = controller.Read();

        // Assert
        Assert.Contains(cheep, records);
    }

    [Fact]
    private async Task Endpoint_Cheep_Stores_CheepInDatabase()
    {
        //Arrange
        using var factory = new WebApplicationFactory<Program>(); //Test version of our app(in-memory) & gets httpclient
        var client = factory.CreateClient(); //returns our said httpclient
        var cheep = new Cheep<string>("Eddie", "I'm in the base!", 12345);

        //Act
        var response = await client.PostAsJsonAsync("/cheep", cheep); //sends request to our endpoint(/cheep)

        //Assert
        Assert.True(response.IsSuccessStatusCode); //We are checking if we get a SuccessStatusCode(200-299)
    }


}
