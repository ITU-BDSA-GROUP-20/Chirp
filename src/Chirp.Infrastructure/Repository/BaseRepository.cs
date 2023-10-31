namespace Chirp.Infrastructure.Repository;

public abstract class BaseRepository
{
    protected ChirpDbContext db;
    protected int PageSize {get; set;}

    public BaseRepository(ChirpDbContext chirpDbContext)
    {
        db = chirpDbContext;
        PageSize = 32;
    }

}