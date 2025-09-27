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
}
