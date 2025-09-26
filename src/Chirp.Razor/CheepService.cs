using System.Globalization;

namespace Chirp.Razor;

public interface ICheepService
{
    public List<CheepViewModel> GetCheeps();
    public List<CheepViewModel> GetCheepsFromAuthor(string author);
}

public class CheepService : ICheepService
{
    private readonly DbFacade _dbFacade;

    public CheepService()
    {
        _dbFacade = new DbFacade();
    }

    public List<CheepViewModel> GetCheeps()
    {
        return _dbFacade.GetCheeps();
    }

    public List<CheepViewModel> GetCheepsFromAuthor(string author)
    {
        return _dbFacade.GetCheepsFromAuthor(author);
    }
}
