namespace Chirp.Razor;

public interface ICheepService
{
    public List<CheepViewModel> GetCheeps(int page);
    public List<CheepViewModel> GetCheepsFromAuthor(string author, int page);
}

public class CheepService : ICheepService
{
    private readonly DbFacade _dbFacade = new();

    public List<CheepViewModel> GetCheeps(int page)
    {
        return _dbFacade.GetCheeps(page);
    }

    public List<CheepViewModel> GetCheepsFromAuthor(string author, int page)
    {
        return _dbFacade.GetCheepsFromAuthor(author, page);
    }
}
