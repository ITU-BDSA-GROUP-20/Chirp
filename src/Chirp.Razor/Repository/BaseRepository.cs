namespace Chirp.Razor.Repository;

public abstract class BaseRepository
{
    protected ChirpDBContext db;
    protected const int PageSize = 32;

    public BaseRepository(ChirpDBContext chirpDbContext)
    {
        db = chirpDbContext;
    }
}