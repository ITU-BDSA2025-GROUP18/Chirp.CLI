
namespace Chirp.Razor;

#nullable disable

public class CheepDTO
{
    public string AuthorName;
    public string Text;
    public string Timestamp;
}

#nullable restore

public interface ICheepRepository
{
    public Task GetCheepsAsync(int page);
    public Task GetCheepsFromAuthorAsync(string author, int page);
}

public class CheepRepository : ICheepRepository
{
    private readonly ChirpDBContext _dbContext;

    public CheepRepository(ChirpDBContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task GetCheepsAsync(int page)
    {
        throw new NotImplementedException();
    }

    public Task GetCheepsFromAuthorAsync(string author, int page)
    {
        throw new NotImplementedException();
    }
}
