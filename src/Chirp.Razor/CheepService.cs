using System.Data.SqlTypes;
using Chirp.Razor;
using Chirp.Razor.Models;
using Chirp.Razor.Repository;
using Microsoft.VisualBasic;

public record CheepViewModel(string Author, string Message, string Timestamp);


public interface ICheepService
{
    public List<CheepViewModel> GetCheeps(int page);
    public List<CheepViewModel> GetCheepsFromAuthor(string author, int page);
}

public class CheepService : ICheepService
{
    
    private IAuthorRepository _Author = new AuthorRepository();
    private readonly ICheepRepository _Cheep = new CheepRepository();

    public CheepService()
    {
       
    }
    
    public List<CheepViewModel> GetCheeps(int page)
    {
        return _Cheep.GetCheepsByPage(page);;
    }
    /* private List<CheepViewModel> FormatCheeps(List<Cheep> unformattedList){

       var _cheeps = new List<CheepViewModel>();

       for(int i = 0; i<unformattedList.Count; i++){
           var cheep = unformattedList[i];
           _cheeps.Add(new CheepViewModel(
              cheep.Author.Name,
                cheep.Text,
                UnixTimeStampToDateTimeString(Convert.ToDouble(cheep.TimeStamp))));
        }
        return _cheeps;
     }*/

    public List<CheepViewModel> GetCheepsFromAuthor(string author, int page)
    {
        var _author = _Author.GetAuthorByName(author);
        return new List<CheepViewModel>();
    }

   /* private static string UnixTimeStampToDateTimeString(double unixTimeStamp)
    {
        // Unix timestamp is seconds past epoch
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(unixTimeStamp);
        return dateTime.ToString("MM/dd/yy H:mm:ss");
    }
   */
}