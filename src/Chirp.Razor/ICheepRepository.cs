using Chirp.Razor.Models;

namespace Chirp.Razor;

public interface ICheepRepository
{
    public IEnumerable<CheepViewModel> GetCheepsByPage(int page);
    public void DeleteCheepById(int cheepId);
    public void AddCheep(Cheep cheep);
    

}