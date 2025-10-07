using Xunit;

public class ApiTests : IClassFixture<WebAppFactory>, IDisposable
{
    private readonly HttpClient _client;
    private readonly string _dbPath;

    public ApiTests(WebAppFactory factory)
    {
        _dbPath = TestDb.CreateWithSeed();
        Environment.SetEnvironmentVariable("CHIRPDBPATH", _dbPath);
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Root_ShouldContain_Helge_Message()
    {
        var res = await _client.GetAsync("/");
        res.EnsureSuccessStatusCode();
        var html = await res.Content.ReadAsStringAsync();
        Assert.Contains("Helge", html);
        Assert.Contains("Hello, BDSA students!", html);
    }

    [Fact]
    public async Task Adrian_ShouldContain_Adrian_Message()
    {
        var res = await _client.GetAsync("/Adrian");
        res.EnsureSuccessStatusCode();
        var html = await res.Content.ReadAsStringAsync();
        Assert.Contains("Adrian", html);
        Assert.Contains("Hej, velkommen til kurset.", html);
    }

    public void Dispose()
    {
        try { File.Delete(_dbPath); } catch { }
        Environment.SetEnvironmentVariable("CHIRPDBPATH", null);
    }
}
