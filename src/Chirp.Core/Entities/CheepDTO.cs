namespace Chirp.Core.Entities;

public class CheepDTO 
{
    public int CheepId {get; set;}
    public int AuthorId {get; set;}
    public AuthorDTO AuthorDto {get; set;}
    public string Text {get; set;}
    public DateTime TimeStamp {get; set;}

}