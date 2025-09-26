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
    public void Cheep_Then_Read_ShouldReturnMessage()
    {
        // Arrange
        /*We no longer need to arrange a database since it
         is in the cloud which is our azure webpage*/
        var controller = new Controller();

        // Act: send a cheep
        controller.Run(new[] { "cheep", "Hello world!" });

        // Act: read back messages
        using var sw = new StringWriter();
        Console.SetOut(sw);

        controller.Run(new[] { "read", "1" });

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
        var ex = Record.Exception(() => controller.Run(new[] { command, message }));
        Assert.Null(ex);
    }

    [Fact]
    public void Read_MoreThanStored_ShouldHandleGracefully()
    {
        // Arrange
        /*We no longer need to arrange a database since it
         is in the cloud which is our azure webpage*/
        var controller = new Controller();

        // Store only one cheep
        controller.Run(new[] { "cheep", "Only one" });

        using var sw = new StringWriter();
        Console.SetOut(sw);

        // Act: try to read 10
        controller.Run(new[] { "read", "10" });
        var output = sw.ToString();

        // Assert: Still should print one, not throw
        Assert.Contains("Only one", output);
    }
}


