namespace Chirp.CLI;

public record Cheep(string Author, string Message, long Timestamp)
{
    public override string ToString()
    {
        DateTime time = DateTimeOffset.FromUnixTimeSeconds(Timestamp).LocalDateTime;
        string timeFormatted = time.ToString(System.Globalization.CultureInfo.CreateSpecificCulture("fr-FR"));
        return $"{Author} @ {timeFormatted} {Message}";
    } 
}


/*public class Cheep
{
    private string author;
    private string message;
    private long unixtimestamp;

    public Cheep(string message)
    {
        this.message = message;
        author = Environment.UserName;

        //Src: "Andrew" https://stackoverflow.com/questions/17632584/how-to-get-the-unix-timestamp-in-c-sharp
        unixtimestamp = (long)DateTime.Now.Subtract(DateTime.UnixEpoch).TotalSeconds;
    }

    public string toString()
    {
        DateTimeOffset timestamp = DateTimeOffset.FromUnixTimeMilliseconds(unixtimestamp);

        return $"{author},\"{message}\",{timestamp}";
    }
}*/