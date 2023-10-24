namespace Chirp.Infrastructure.Repository;

public abstract class BaseRepository
{
    protected ChirpDbContext db;
    protected const int PageSize = 32;

    public BaseRepository(ChirpDbContext chirpDbContext)
    {
        db = chirpDbContext;
    }
}