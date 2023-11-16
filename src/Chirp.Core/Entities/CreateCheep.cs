using FluentValidation;

namespace Chirp.Core.Entities;

public record CreateCheep(Author Author, string Text);

