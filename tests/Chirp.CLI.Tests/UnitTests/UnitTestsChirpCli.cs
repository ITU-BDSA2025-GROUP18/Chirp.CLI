using System.CommandLine;
using System.Text.Json;

namespace Chirp.CLI.Tests;

public class UnitTestsChirpCli
{
    // This test simulated what happens when cheep is uploaded to the web server
    [Fact]
    public void CheepsShouldSerializeAndDeserialize()
    {
        // Arrange
        var cheep = new Cheep<string>("Alice", "Lorem ipsum", 12345);

        // Act
        var json = JsonSerializer.Serialize(cheep);
        var jsonDeserialized = JsonSerializer.Deserialize<Cheep<string>>(json);

        // Assert
        Assert.Equal(cheep, jsonDeserialized);
    }

    // This test makes sure that the UserInterface static method PrintCheeps correctly prints cheeps to the console
    [Fact]
    public void CheepsShouldBePrintedInTheConsole()
    {
        // Arrange
        var cheeps = new List<Cheep<string>>
        {
            new Cheep<string>("Alice", "Lorem ipsum lorem ipsum testing testing", 12345)
        };

        using var sw = new StringWriter(); // Fake console
        Console.SetOut(sw); // Routes stuff to the fake console

        // Act
        UserInterface<Cheep<string>>.PrintCheeps(cheeps);

        // Assert
        var output = sw.ToString();
        Assert.Contains("Lorem ipsum lorem ipsum testing testing", output); // Fake console should contain
    }
}
