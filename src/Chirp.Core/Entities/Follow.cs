using System.ComponentModel.DataAnnotations;

namespace Chirp.Core.Entities;

public class Follow
{
    public Guid FollowerId { get; set; }
    [Required]
    public Author Follower { get; set; }
    public Guid FollowingId { get; set; }
    [Required]
    public Author Following { get; set; }
}