using Chirp.Core.Entities;

namespace Chirp.Core.Repository;

public interface ICheepRepository
{
    public ICollection<Cheep> GetCheepsByPage(int page);
    public void AddCheep(Cheep cheepDto);
    
}