namespace Chirp.Core.Entities;

public class Follow
{
    public Guid FollowerId { get; set; }
    public Author Follower { get; set; }

    public Guid FollowingId { get; set; }
    public Author Following { get; set; }
}