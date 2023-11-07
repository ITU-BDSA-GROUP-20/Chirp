using FluentValidation;

namespace Chirp.Core.Entities;

public record CreateCheepDTO(string Author, string Text);

