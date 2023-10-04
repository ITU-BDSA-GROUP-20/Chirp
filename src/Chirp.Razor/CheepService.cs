using Chirp.SQLite;
using Microsoft.VisualBasic;

public record CheepViewModel(string Author, string Message, string Timestamp);


public interface ICheepService
{
    public List<CheepViewModel> GetCheeps(int page);
    public List<CheepViewModel> GetCheepsFromAuthor(string author, int page);
}

public class CheepService : ICheepService
{
    DBFacade DB = new();
    

    public List<CheepViewModel> GetCheeps(int page)
    {
        //query ran:
        //SELECT user.username, message.text, message.pub_date FROM message JOIN user on message.author_id = user.user_id ORDER by message.pub_date desc
        

        List<Object> unrefinedList = DB.GetAllMessages(page);

        return FormatCheeps(unrefinedList);
        
    }
    private List<CheepViewModel> FormatCheeps(List<Object> unformattedList){
        var _cheeps = new List<CheepViewModel>();
        for(int i = 0; i<unformattedList.Count-2; i+=3){

            _cheeps.Add(new CheepViewModel(
                //Needs to conform to order of how we retrieve data from the query
                unformattedList[i].ToString()!,
                unformattedList[i+1].ToString()!,
                UnixTimeStampToDateTimeString(Convert.ToDouble(unformattedList[i+2]))));
        }
        return _cheeps;
    }

    public List<CheepViewModel> GetCheepsFromAuthor(string author, int page)
    {
        //query ran:
        //SELECT user.username, message.text, message.pub_date FROM message JOIN user ON message.author_id = user.user_id WHERE user.username = @Author ORDER by message.pub_date desc
       
        List<Object> unformattedList = DB.getAuthorsMessages(author, page);
        // filter by the provided author name
        return FormatCheeps(unformattedList);
    }

    private static string UnixTimeStampToDateTimeString(double unixTimeStamp)
    {
        // Unix timestamp is seconds past epoch
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(unixTimeStamp);
        return dateTime.ToString("MM/dd/yy H:mm:ss");
    }

}
