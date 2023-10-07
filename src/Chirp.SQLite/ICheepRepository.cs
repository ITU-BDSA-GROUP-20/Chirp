namespace Chirp.SQLite;

public interface ICheepRepository
{
    public IEnumerable<Cheep> GetCheepsByPage(int page);
    public void DeleteCheepByID(int cheepID);
    public void AddCheep(Cheep cheep);
    

}