using SimpleDB;

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

    public void DoesChirpStoreDataCorrectly()
    /*This will be a test where we check if data is stored correctly into the database.
    We will test the following message: "Hello!!!" will be stored in the database together with
    its author and timestamp. 
    We'll therefore create a new CSVDatabase that points to our temporary file &
    create a controller that uses our new CSVDatabase  */
    {
        var db = new CSVDatabase<Cheep<T>>(_dbPath);
        var controller = new Controller(db);

        controller.Run(new[] { "cheep", "Hello!!!" }); /*"cheep" = our keyword for our parser to read
        "Hello!!!" = message we want to cheep*/
        
        //Assertion
        /*First we check that we have our cheep stored. (2 lines because first line
        is our header from earlier*/
        var lines = File.ReadAllLines(_dbPath);
        Assert.Equal(2, lines.Length);
        
        //now what am i missing
        Assert.Contains("Hello!!!", x);
    }
}