namespace Chirp.CSVDBService;

public class CheepController
{
    public CheepController(WebApplication app)
    {
        /// ======== GET ======== ///
        app.MapGet("/cheep", () => "Cheep");
    }
}
