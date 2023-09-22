

using CSVDBService;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

IDatabaseRepository<Cheep> database = CSVDatabase<Cheep>.Instance;
app.MapGet("/cheeps", () => new Cheep("Mads", "Hello World!", 5485489798799));//database.Read(10));

app.Run();


public record Cheep(string Author, string Message, long Timestamp);
