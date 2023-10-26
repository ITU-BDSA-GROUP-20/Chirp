namespace Chirp.Core.Entities;

public class CheepDTO 
{
    public Guid CheepId {get; set;}
    public Guid AuthorId {get; set;}
    public AuthorDTO AuthorDto {get; set;}
    public string Text {get; set;}
    public DateTime TimeStamp {get; set;}

}