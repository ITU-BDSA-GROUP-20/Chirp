using Chirp.Core.Entities;
using FluentValidation;

namespace Chirp.Infrastructure.Repository;

public class CheepCreateValidator : AbstractValidator<CreateCheepDTO>
{
    public CheepCreateValidator()
    {
        // @TODO Check that these values are correct: 
        RuleFor(x => x.Text).NotEmpty().MaximumLength(128);
        RuleFor(x => x.Author).NotEmpty().MaximumLength(50);
    }
    
}