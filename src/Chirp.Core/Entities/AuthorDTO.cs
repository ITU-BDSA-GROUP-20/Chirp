using System.Collections;

namespace Chirp.Core.Entities;
public class AuthorDTO {
    public int AuthorId {get; set;}
    public string Name {get; set;}
    public string Email {get; set;}
    public ICollection<CheepDTO> Cheeps {get; set;}
}