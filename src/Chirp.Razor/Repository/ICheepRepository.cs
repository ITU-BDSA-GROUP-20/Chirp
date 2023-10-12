using Chirp.Razor.Models;

namespace Chirp.Razor.Repository;

public interface ICheepRepository
{
    public List<CheepViewModel> GetCheepsByPage(int page);
    public void AddCheep(Cheep cheep);
    

}