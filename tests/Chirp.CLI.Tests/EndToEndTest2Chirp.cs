namespace Chirp.CLI.Tests;

public class EndToEndTest2Chirp : IDisposable
{
    //Lets first create a temporary csv-file which we will use for this test.
    private readonly string _dbPath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
    
    //But let's also ensure that a csv-file with this name doesn't currently exist.
    public void Dispose()
    {
        if (File.Exists(_dbPath))
        {
            File.Delete(_dbPath);
        }
    }
    
    public ChirpEndToEndTests()
    {
        // Makes sure the CSV-file starts with the correct headers. (Thanks oli)
        File.WriteAllText(_dbPath, "Author,Message,Timestamp\n");
    }
}