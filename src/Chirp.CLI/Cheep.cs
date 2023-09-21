namespace Chirp.CLI;

public record Cheep(string Author, string Message, long Timestamp)
{
    public override string ToString()
    {
        DateTime time = DateTimeOffset.FromUnixTimeSeconds(Timestamp).LocalDateTime;
        string timeFormatted = time.ToString();
        return $"{Author} @ {timeFormatted} {Message}";
    } 
}