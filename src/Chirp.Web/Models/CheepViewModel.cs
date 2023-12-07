using Chirp.Core.Entities;

namespace Chirp.Web.Models;

public record CheepViewModel(UserModel User, string Message, string Timestamp, ICollection<ReactionDTO> Reactions);
