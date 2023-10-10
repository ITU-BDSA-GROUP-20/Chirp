namespace Chirp.Razor.Models;
public class Cheep 
{
    public int CheepId {get; set;}
    public int AuthorId {get; set;}
    public Author Author {get; set;}
    public string Text {get; set;}
    public string TimeStamp {get; set;}
}