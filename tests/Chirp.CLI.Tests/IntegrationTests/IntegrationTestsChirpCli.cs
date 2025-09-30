using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

namespace Chirp.CLI.Tests.IntegrationTests;

public class ChirpCliIntegrationTests : IAsyncLifetime
{
    private IHost? _server;
    private const string _testUrl = "http://localhost:7070";

    public async Task InitializeAsync()
    {
        var builder = WebApplication.CreateBuilder();
        var app = builder.Build();

        var repository = new CSVDBService.Controllers.CheepRepository<CSVDBService.Models.Cheep<string>>();
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
    }

    [Fact]
    public async Task Cheep_Then_Read_ShouldReturnMessage()
    {
        // Arrange
        var controller = new CLI.Controller(_testUrl);

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

        // Act + Assert: Should not throw
        var ex = await Record.ExceptionAsync(() => controller.Run([command, message]));
        Assert.Null(ex);
    }

    [Fact]
    public async Task Read_MoreThanStored_ShouldHandleGracefully()
    {
        // Arrange
        var controller = new CLI.Controller(_testUrl);

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
