using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Core.Entities;

[Index(nameof(CheepId), IsUnique = true)]
public class CheepDTO 
{
    [Required]
    public int CheepId {get; set;}
    
    [Required]
    public int AuthorId {get; set;}
    
    [Required]
    public AuthorDTO AuthorDto {get; set;}

    [StringLength(128)] [Required] 
    public string Text { get; set; } = "";
    
    [Required]
    public DateTime TimeStamp {get; set;}

}