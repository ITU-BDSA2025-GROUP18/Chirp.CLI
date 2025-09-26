using System;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Xunit;

namespace Chirp.Tests;

// ---- TESTING INFORMATION ---- //
// These test should run against the deployed app on Azure.
// But since we have shut down Azure webapp, we are now using a simulated webserver on localhost.

public class ChirpEndToEndTests : IAsyncLifetime
{
    private IHost? _server;
    private const string _testUrl = "http://localhost:8080";

    public async Task InitializeAsync()
    {
        var builder = WebApplication.CreateBuilder();
        var app = builder.Build();

        var repository = new CSVDBService.Controllers.CheepRepository<CSVDBService.Models.Cheep<string>>();
        _ = new CSVDBService.CheepController(app, repository);

        _server = app;

        // Run the server non-blocking
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

        await controller.Run(["read", "1"]);

        var output = sw.ToString();

        // Assert: Should not throw
        Assert.Contains("Hello world!", output);
        Assert.Contains(Environment.UserName, output);
    }


    [Theory]
    [InlineData("cheep", "")]
    [InlineData("cheep", "🔥 fuzz input with unicode!")]
    [InlineData("cheep", "DROP TABLE chirps;")]
    public void Cheep_WithVariousInputs_ShouldNotCrash(string command, string message)
    {
        // Arrange
        var controller = new CLI.Controller(_testUrl);

        // Act + Assert: Should not throw
        var ex = Record.ExceptionAsync(() => controller.Run([command, message]));
        Assert.Null(ex);
    }

    [Fact]
    public async Task Read_MoreThanStored_ShouldHandleGracefully()
    {
        // Arrange
        /*We no longer need to arrange a database since it
         is in the cloud which is our azure webpage */
        var controller = new CLI.Controller("http://localhost:8080");

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
