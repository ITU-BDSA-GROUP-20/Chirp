using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Core.Entities;

[Index(nameof(CheepId), IsUnique = true)]
public class Cheep 
{
    [Required]
    public Guid CheepId {get; set;}
    
    [Required]
    public Guid AuthorId {get; set;}
    
    [Required]
    public required Author Author {get; set;}

    [StringLength(128, MinimumLength = 5)] [Required] 
    public string Text { get; set; } = "";
    
    [Required]
    public DateTime TimeStamp {get; set;}
}