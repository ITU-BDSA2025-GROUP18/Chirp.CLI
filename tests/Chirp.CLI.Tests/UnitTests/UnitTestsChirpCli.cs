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

    // Test
}
