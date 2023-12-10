using Chirp.Core.Entities;
using Chirp.Core.Repository;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace Chirp.Web.Pages;

public class PublicModel : TimeLineModel
{
    public PublicModel(ICheepService cheepService,
        ICheepRepository cheepRepository,
        IAuthorRepository authorRepository,
        IReactionRepository reactionRepository,
        IValidator<CreateCheep> cheepValidator,
        UserManager<Author> userManager) : base(
        cheepService,
        cheepRepository,
        authorRepository,
        reactionRepository,
        cheepValidator,
        userManager
    )
    {
        
    }

    protected override void InitializeCheepList()
    {
        Cheeps = _cheepService.GetCheeps(CurrentPage);
    }
}