using Chirp.Core.Entities;

namespace Chirp.Core.Repository;

public interface ICheepRepository
{
    public ICollection<CheepDTO> GetCheepsByPage(int page);
    public void AddCheep(CheepDTO cheepDto);
    
}