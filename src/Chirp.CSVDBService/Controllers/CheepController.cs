using Chirp.CSVDBService.Controllers;

namespace Chirp.CSVDBService;

public class CheepController
{
    
    public CheepController(WebApplication app, CheepRepository repository)
    {
        /// ======== GET ======== ///
        
        // Get all cheeps 
        app.MapGet("/cheeps", (int? limit=null) => repository.Read(limit));
        
        
    }
}
