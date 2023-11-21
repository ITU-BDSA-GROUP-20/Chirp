using Chirp.Core.Entities;

namespace Chirp.Core.Repository;

public interface ICheepRepository
{
    public ICollection<Cheep> GetCheepsByPage(int page);
    public Task AddCheep(CreateCheep cheepDto);
    public Cheep CreateCheep2Cheep(CreateCheep cheep);

}