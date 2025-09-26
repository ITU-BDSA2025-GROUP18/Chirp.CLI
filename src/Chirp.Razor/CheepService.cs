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

    // These would normally be loaded from a database for example
    private static readonly List<CheepViewModel> _cheeps = new()
        {
            new CheepViewModel("Helge", "Hello, BDSA students!", UnixTimeStampToDateTimeString(1690892208)),
            new CheepViewModel("Adrian", "Hej, velkommen til kurset.", UnixTimeStampToDateTimeString(1690895308)),
        };

    public List<CheepViewModel> GetCheeps()
    {
        return _dbFacade.GetCheeps();
    }

    public List<CheepViewModel> GetCheepsFromAuthor(string author)
    {
        // filter by the provided author name
        //return _cheeps.Where(x => x.Author == author).ToList();
        return _dbFacade.GetCheepsFromAuthor(author);
    }

    private static string UnixTimeStampToDateTimeString(long unixTimeStamp)
    {
        var formattedTimeStamp = DateTimeOffset
            .FromUnixTimeSeconds(unixTimeStamp)
            .LocalDateTime
            .ToString(CultureInfo.InvariantCulture);

        return formattedTimeStamp;
    }

}
