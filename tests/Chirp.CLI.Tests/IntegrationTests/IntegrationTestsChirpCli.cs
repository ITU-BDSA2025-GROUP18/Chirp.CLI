using System.Globalization;
using Chirp.CSVDBService.Controllers;
using CsvHelper;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

namespace Chirp.CLI.Tests.IntegrationTests;

public class ChirpCliIntegrationTests : IAsyncLifetime
{
    private readonly string _tempFile = Path.GetTempFileName(); // temporary csv db for testing
    private IHost? _server;
    private const string _testUrl = "http://localhost:7070";

    public async Task InitializeAsync()
    {
        var builder = WebApplication.CreateBuilder();
        var app = builder.Build();

        var repository = new CheepRepository<CSVDBService.Models.Cheep<string>>(_tempFile);
        _ = new CSVDBService.CheepController(app, repository);

        _server = app;

        _ = app.RunAsync(_testUrl);

        await Task.Delay(500);
    }

    public async Task DisposeAsync()
    {
        if (_server != null)
        {
            await _server.StopAsync();
            _server.Dispose();
        }

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
    public async Task Cheep_Then_Read_ShouldReturnMessage()
    {
        // Arrange
        var controller = new CLI.Controller(_testUrl);
        ArrangeCsv();

        // Act: send a cheep
        await controller.Run(["cheep", "Hello world!"]);

        // Act: read back messages
        await using var sw = new StringWriter();
        Console.SetOut(sw);

        await controller.Run(["read"]);

        var output = sw.ToString();

        // Assert: Should not throw
        Assert.Contains("Hello world!", output);
        Assert.Contains(Environment.UserName, output);
    }

    [Theory]
    [InlineData("cheep", "")]
    [InlineData("cheep", "🔥 fuzz input with unicode!")]
    [InlineData("cheep", "DROP TABLE chirps;")]
    public async Task Cheep_WithVariousInputs_ShouldNotCrash(string command, string message)
    {
        // Arrange
        var controller = new CLI.Controller(_testUrl);
        ArrangeCsv();

        // Act + Assert: Should not throw
        var ex = await Record.ExceptionAsync(() => controller.Run([command, message]));
        Assert.Null(ex);
    }

    [Fact]
    public async Task Read_MoreThanStored_ShouldHandleGracefully()
    {
        // Arrange
        var controller = new CLI.Controller(_testUrl);
        ArrangeCsv();

        // Store only one cheep
        await controller.Run(new[] { "cheep", "Only one" });

        using var sw = new StringWriter();
        Console.SetOut(sw);

        // Act: try to read 10
        await controller.Run(new[] { "read" });
        var output = sw.ToString();

        // Assert: Still should print one, not throw
        Assert.Contains("Only one", output);
    }
}
