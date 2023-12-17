using System.ComponentModel.DataAnnotations;

namespace Chirp.Core.Entities;

public class Follow
{
    [Required]
    public Guid FollowingAuthorId { get; set; }
    [Required]
    public Author? FollowingAuthor { get; set; }
    [Required]
    public Guid FollowedAuthorId { get; set; }
    [Required]
    public Author? FollowedAuthor { get; set; }
}