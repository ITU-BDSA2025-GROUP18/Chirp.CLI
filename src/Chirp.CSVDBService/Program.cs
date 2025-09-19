var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/cheeps", () => new Cheep("me", "Hej!", 1684229348));

app.Run();