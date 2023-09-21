

using CSVDBService;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

IDatabaseRepository<Cheep> database = CSVDatabase<Cheep>.Instance;
app.MapGet("/cheeps", () => database.Read(10));

app.Run();


public record Cheep(string Author, string Message, long Timestamp);
