namespace Chirp.Razor;

public interface ICheepService
{
    public List<CheepViewModel> GetCheeps(int page);
    public List<CheepViewModel> GetCheepsFromAuthor(string author);
}

public class CheepService : ICheepService
{
    private readonly DbFacade _dbFacade = new();

    public List<CheepViewModel> GetCheeps(int page)
    {
        return _dbFacade.GetCheeps(page);
    }

    public List<CheepViewModel> GetCheepsFromAuthor(string author)
    {
        return _dbFacade.GetCheepsFromAuthor(author);
    }
}
