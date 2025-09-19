using Chirp.CSVDBService;
using Chirp.CSVDBService.Controllers;


var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
var repository = new CheepRepository();
_ = new CheepController(app, repository);

app.Run();