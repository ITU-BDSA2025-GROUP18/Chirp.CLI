namespace Chirp.CLI.Tests;

public class EndtoEndTestsChirp
{
    [Fact]
    public void TestReadTenCheeps()
    {
        // Arrange
        var args = new string[] { "read", "10" };
        // Act
        var result = Program.Main(args);
        // Assert
        Assert.Equal(0, result);
    }
}