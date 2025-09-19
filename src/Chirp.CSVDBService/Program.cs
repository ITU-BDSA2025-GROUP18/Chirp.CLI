using Chirp.CSVDBService;
using Chirp.CSVDBService.Controllers;
using Chirp.CSVDBService.Models;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
var repository = new CheepRepository<Cheep<string>>();
_ = new CheepController(app, repository);

app.Run();