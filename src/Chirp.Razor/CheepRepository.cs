
using System.Globalization;
using Microsoft.EntityFrameworkCore;

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
    public Task<List<CheepDTO>> GetCheepsAsync(int page);
    public Task<List<CheepDTO>> GetCheepsFromAuthorAsync(string author, int page);
}

public class CheepRepository : ICheepRepository
{
    private readonly ChirpDBContext _dbContext;

    public CheepRepository(ChirpDBContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<CheepDTO>> GetCheepsAsync(int page)
    {
        var query = _dbContext.Cheeps
            .OrderByDescending(cheep => cheep.TimeStamp)
            .Skip((page - 1) * 32)
            .Take(32)
            .Select(cheep => new CheepDTO
            {
                AuthorName = cheep.Author.Name,
                Text = cheep.Text,
                Timestamp = TimeStampToLocalTimeString(cheep.TimeStamp)
            });

        return await query.ToListAsync();
    }

    public async Task<List<CheepDTO>> GetCheepsFromAuthorAsync(string author, int page)
    {
        var query = _dbContext.Cheeps
            .Where(cheep => cheep.Author.Name == author)
            .OrderByDescending(cheep => cheep.TimeStamp)
            .Skip((page - 1) * 32)
            .Take(32)
            .Select(cheep => new CheepDTO
            {
                AuthorName = cheep.Author.Name,
                Text = cheep.Text,
                Timestamp = TimeStampToLocalTimeString(cheep.TimeStamp)
            });

        return await query.ToListAsync();
    }

    private static string TimeStampToLocalTimeString(DateTime timestamp)
    {
        return timestamp.ToLocalTime().ToString(CultureInfo.InvariantCulture);
    }
}
