namespace Chirp.Razor;

public abstract class BaseRepository
{
    protected ChirpDbContext db;
    protected const int PageSize = 32;
}