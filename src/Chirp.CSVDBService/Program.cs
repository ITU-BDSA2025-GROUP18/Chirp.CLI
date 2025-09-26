using Chirp.CSVDBService.Controllers;
using Chirp.CSVDBService.Models;

namespace Chirp.CSVDBService;

public class Program()
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var app = builder.Build();

        app.Urls.Add("http://localhost:8080");

        var repository = new CheepRepository<Cheep<string>>();
        _ = new CheepController(app, repository);

        app.Run();
    }
}
