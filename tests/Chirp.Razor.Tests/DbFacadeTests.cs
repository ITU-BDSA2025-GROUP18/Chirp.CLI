using Chirp.Razor;
using Xunit;

public class DbFacadeTests : IDisposable
{
    private readonly string _dbPath;

    public DbFacadeTests()
    {
        _dbPath = TestDb.CreateWithSeed();
        Environment.SetEnvironmentVariable("CHIRPDBPATH", _dbPath);
    }

    [Fact]
    public void GetPublicTimeline_Returns_Helge_Cheep()
    {
        var facade = new DbFacade();
        var cheeps = facade.GetCheeps(page: 1);
        Assert.Contains(cheeps, c => c.Author == "Helge" && c.Message.Contains("Hello, BDSA students!"));
        facade.CloseConnection();
    }

    [Fact]
    public void GetUserTimeline_Returns_Adrian_Cheep()
    {
        var facade = new DbFacade();
        var cheeps = facade.GetCheepsFromAuthor("Adrian", page: 1);
        Assert.Contains(cheeps, c => c.Author == "Adrian" && c.Message.Contains("Hej, velkommen til kurset."));
        facade.CloseConnection();
    }

    public void Dispose()
    {
        try { File.Delete(_dbPath); } catch { }
        Environment.SetEnvironmentVariable("CHIRPDBPATH", null);
    }
}
