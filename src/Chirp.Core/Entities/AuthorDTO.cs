using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Core.Entities;

[Index(nameof(AuthorId), IsUnique = true)]
public class AuthorDTO {
    
    [Unicode]
    public int AuthorId {get; set;}
    
    [StringLength(50)]
    [Required]
    public string Name {get; set;}
    
    [StringLength(50)]
    [Required]
    public string Email {get; set;}
    public ICollection<CheepDTO> Cheeps {get; set;}
}