using Chirp.Core.Entities;
using Chirp.Core.Repository;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace Chirp.Web.Pages;

public class UserTimelineModel : TimeLineModel
{
    public UserTimelineModel(ICheepService cheepService,
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
        Author timelineAuthor = _authorRepository.GetAuthorByName(PageContext.RouteData.Values["author"].ToString());
        Cheeps = _cheepService.GetCheepsFromAuthor(timelineAuthor.Id, CurrentPage);
    }
}
