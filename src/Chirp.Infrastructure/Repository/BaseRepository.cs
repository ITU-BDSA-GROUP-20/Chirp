namespace Chirp.Infrastructure.Repository;

public abstract class BaseRepository
{
    protected ChirpDbContext db;
    protected int PageSize {get; set;}

    public BaseRepository(ChirpDbContext chirpDbContext, int pageSize)
    {
        db = chirpDbContext;
        PageSize = pageSize;
    }

}