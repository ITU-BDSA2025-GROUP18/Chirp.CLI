using System;
using System.IO;
using System.Linq;
using Chirp.CLI;
using Xunit;

namespace Chirp.Tests;

public class ChirpEndToEndTests : IDisposable
{
    private readonly string _dbPath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());

    public ChirpEndToEndTests()
    {
        // Sørger for at filen starter med headers
        File.WriteAllText(_dbPath, "Author,Message,Timestamp\n");
    }

    public void Dispose()
    {
        if (File.Exists(_dbPath))
        {
            File.Delete(_dbPath);
        }
    }

    [Fact]
    public async Task Cheep_Then_Read_ShouldReturnMessage()
    {
        // Arrange
        /*We no longer need to arrange a database since it
         is in the cloud which is our azure webpage*/
        var controller = new Controller();

        // Act: send a cheep
        await controller.Run(new[] { "cheep", "Hello world!" });

        // Act: read back messages
        using var sw = new StringWriter();
        Console.SetOut(sw);

        await controller.Run(new[] { "read", "1"}); //den hed "read", "1" før

        var output = sw.ToString();

        // Assert
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
        /*We no longer need to arrange a database since it
         is in the cloud which is our azure webpage*/
        var controller = new Controller();

        // Act + Assert: Should not throw
        var ex = Record.ExceptionAsync(() => controller.Run(new[] { command, message }));
        Assert.Null(ex);
    }

    [Fact]
    public async Task Read_MoreThanStored_ShouldHandleGracefully()
    {
        // Arrange
        /*We no longer need to arrange a database since it
         is in the cloud which is our azure webpage*/
        var controller = new Controller();

        // Store only one cheep
        await controller.Run(new[] { "cheep", "Only one" });

        using var sw = new StringWriter();
        Console.SetOut(sw);

        // Act: try to read 10
        await controller.Run(new[] { "read"});
        var output = sw.ToString();

        // Assert: Still should print one, not throw
            Assert.Contains("Only one", output);
    }
}


