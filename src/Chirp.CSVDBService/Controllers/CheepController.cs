using Chirp.CSVDBService.Controllers;
using Chirp.CSVDBService.Models;

namespace Chirp.CSVDBService;

public class CheepController
{
    
    public CheepController(WebApplication app, CheepRepository<Cheep<string>> repository)
    {
        /// ======== GET ======== ///
        
        // Get all cheeps 
        app.MapGet("/cheeps", (int? limit=null) => repository.Read(limit));
        
        
    }
}
