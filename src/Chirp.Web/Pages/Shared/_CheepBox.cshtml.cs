using System.ComponentModel.DataAnnotations;
using Chirp.Core.Entities;
using Chirp.Core.Repository;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ValidationException = FluentValidation.ValidationException;

namespace Chirp.Web.Pages.Shared;

public class CheepBoxModel : PageModel
{
    private readonly UserManager<Author> _userManager;
    private readonly IValidator<CreateCheep> _validator;
    private readonly ICheepRepository _cheepRepository;

    public CheepBoxModel(UserManager<Author> userManager, IValidator<CreateCheep> validator, ICheepRepository cheepRepository)
    {
        _userManager = userManager;
        _validator = validator;
        _cheepRepository = cheepRepository;
    }

    [BindProperty] 
    public NewCheep NewCheep { get; set; }

    public async Task OnPost()
    {   
       
        var author = await _userManager.GetUserAsync(User);
        var cheep = new CreateCheep(author!, NewCheep.Text);

        await CreateCheep(cheep);
        RedirectToPage("/@User.Name");
    }
    
    public async Task CreateCheep(CreateCheep newCheep)
    {
        var validationResult = await _validator.ValidateAsync(newCheep);
        if (!validationResult.IsValid)
        {
            throw new ValidationException("The message can be no longer than 128 characters.");
        }
        
        var entity = new Cheep()
        {
            Text = newCheep.Text,
            TimeStamp = DateTime.UtcNow,
            Author = newCheep.Author
        };
        
        _cheepRepository.AddCheep(entity);
    }
}

public class NewCheep
{
    [Required]
    [StringLength(128, MinimumLength = 5)]
    public string Text { get; set; } = "";
}