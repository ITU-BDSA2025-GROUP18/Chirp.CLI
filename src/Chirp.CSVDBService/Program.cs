using Chirp.CSVDBService;
using Chirp.CSVDBService.Models;


var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
var _ = new CheepController(app);

app.Run();