using Chirp.Razor.Models;

namespace Chirp.Razor.Repository;

public interface ICheepRepository
{
    public List<CheepViewModel> GetCheepsByPage(int page);
    public void DeleteCheepById(int cheepId);
    public void AddCheep(Cheep cheep);
    

}