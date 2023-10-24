using Chirp.Core.Entities;

namespace Chirp.Core.Repository;

public interface ICheepRepository
{
    public List<CheepDTO> GetCheepsByPage(int page);
    public void AddCheep(CheepDTO cheepDto);
    

}