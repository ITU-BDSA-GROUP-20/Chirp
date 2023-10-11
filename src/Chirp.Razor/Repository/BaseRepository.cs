namespace Chirp.Razor.Repository;

public abstract class BaseRepository
{
    protected ChirpDBContext db = new();
    protected const int PageSize = 32;
}