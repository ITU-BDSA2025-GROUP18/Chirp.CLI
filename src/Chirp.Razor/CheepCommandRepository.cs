using System.Globalization;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Razor;

#nullable disable

public class CheepCommandRepository
{
    public string AuthorName;
    public string Text;
    public string Timestamp;
}

#nullable restore

public interface ICheepCommandRepository
{
    public Task CommandAsync(String author, string text); //Future commands
    public Task CommandAsync2 (String author, string text); //Future commands
}

/*public class CheepCommandRepository : ICheepCommandRepository // Commands
{
    //Commands here
} */
