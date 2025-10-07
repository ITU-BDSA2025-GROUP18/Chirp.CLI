
using Microsoft.EntityFrameworkCore;

namespace Chirp.Razor;

#nullable disable

public class Cheep
{
    public int CheepId { get; set; }

    public string Text { get; set; }
    public DateTime TimeStamp { get; set; }
    public Author Author { get; set; }
}

public class Author
{
    public int AuthorId { get; set; }

    public string Name { get; set; }
    public string Email { get; set; }
    public ICollection<Cheep> Cheeps { get; set; }
}

#nullable restore

public class ChirpDBContext : DbContext
{
    public DbSet<Cheep> Cheeps { get; set; }
    public DbSet<Author> Authors { get; set; }

    public ChirpDBContext(DbContextOptions<ChirpDBContext> options) : base(options) { }
}
