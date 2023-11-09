using FluentValidation;

namespace Chirp.Core.Entities;

public record CreateCheep(string Author, string Text);

