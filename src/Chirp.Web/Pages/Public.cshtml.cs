using System.ComponentModel.DataAnnotations;
using Chirp.Core.Entities;
using Chirp.Core.Repository;
using Chirp.Web.Models;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ValidationException = FluentValidation.ValidationException;

namespace Chirp.Web.Pages;

public class PublicModel : BasePageModel
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