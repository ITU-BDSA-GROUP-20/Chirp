using Chirp.Infrastructure;
using Chirp.Infrastructure.Repository;

namespace Chirp.Web.Models;

using Chirp.Core.Entities;
using Chirp.Core.Repository;

public class UserModel
{
    public Guid Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public ICollection<Author> Followers { get; set; }
    public ICollection<Author> Following { get; set; }
    

    // Constructor to initialize properties based on an Author entity
    public UserModel(Author author)
    {
        Id = author.Id;
        Username = author.UserName;
        Email = author.Email;
        Followers = author.Followers;
        Following = author.Following;
    }
}


