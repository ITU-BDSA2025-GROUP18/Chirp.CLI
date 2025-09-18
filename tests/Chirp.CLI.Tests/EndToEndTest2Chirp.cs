namespace Chirp.CLI.Tests;

public class EndToEndTest2Chirp : IDisposable
{
    private readonly string _dbPath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
    
    public void Dispose()
    {
        if (File.Exists(_dbPath))
        {
            File.Delete(_dbPath);
        }
    }
    
    public ChirpEndToEndTests()
    {
        File.WriteAllText(_dbPath, "Author,Message,Timestamp\n");
    }
}