using FluentValidation;

namespace Chirp.Core.Entities;

public record CreateCheepDTO(string Author, string Text);

public class CheepCreateValidator : AbstractValidator<CreateCheepDTO>
{
    public CheepCreateValidator()
    {
        // @TODO Check that these values are correct: 
        RuleFor(x => x.Text).NotEmpty().MaximumLength(128);
        RuleFor(x => x.Author).NotEmpty().MaximumLength(50);
    }
    
}