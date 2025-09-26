namespace Chirp.CLI.Tests;

public class IntegrationTestsChirpCLI : IDisposable
{
    private readonly string _dbPath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());

    public IntegrationTestsChirpCLI()
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
/*
    [Fact]
    public async Task Cheep_Then_Read_ShouldReturnMessage()
    {
        // Arrange
        var program = new CSVDBService.Program();
        program.Main(null);
        var controller = new CLI.Controller("http://localhost:8080");

        // Act: send a cheep
        await controller.Run(["cheep", "Hello world!"]);

        // Act: read back messages
        using var sw = new StringWriter();
        Console.SetOut(sw);

        await controller.Run(["read", "1"]); //den hed "read", "1" før

        var output = sw.ToString();

        // Assert
        Assert.Contains("Hello world!", output);
        Assert.Contains(Environment.UserName, output);
    }

    [Fact]
    public void Test1()
    {
    }*/
}
