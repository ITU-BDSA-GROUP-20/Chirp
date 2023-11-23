using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Core.Entities;

[Index(nameof(Id), IsUnique = true)]
public class Author : IdentityUser<Guid> {
    
    [StringLength(50)]
    [Required]
    public override required string UserName {get; set;}
    
    [StringLength(50)]
    [Required]
    public override required string Email {get; set;}

    public ICollection<Cheep> Cheeps { get; set; } = new List<Cheep>();
    
    public ICollection<Reaction>? Reactions { get; set; }
}