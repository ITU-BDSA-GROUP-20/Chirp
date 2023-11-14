using System.ComponentModel.DataAnnotations;
using Chirp.Core.Entities;
using Chirp.Core.Repository;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ValidationException = FluentValidation.ValidationException;

namespace Chirp.Web.Pages;

public class PublicModel : PageModel
{
    private readonly ICheepService _service;
    private readonly ICheepRepository _cheeprepository;
    private readonly IAuthorRepository _authrepository;
    private readonly IValidator<CreateCheep> _validator;
    
    private readonly UserManager<Author> _userManager;
    public ICollection<CheepViewModel> Cheeps { get; set; }
    
   
    public PublicModel(ICheepService service, ICheepRepository cheeprepository, IAuthorRepository authrepository, IValidator<CreateCheep> validator , UserManager<Author> userManager)
    {
        _service = service;
        _cheeprepository = cheeprepository;
        _authrepository = authrepository;
        _validator = validator;
        _userManager = userManager;
    }

    public ActionResult OnGet()
    {
        int page;
       if(Request.Query.ContainsKey("page")){
            page = int.Parse(Request.Query["page"]! );
       } else{
            page = 1;
       }
        Cheeps = _service.GetCheeps(page);
        return Page();
    }
    
    [BindProperty]
    public NewCheep NewCheep { get; set; }

    public async Task OnPost()
    {   
       
        var author = await _userManager.GetUserAsync(User);
        var cheep = new CreateCheep(author, NewCheep.Text);

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
        
        _cheeprepository.AddCheep(entity);
    }
}

public class NewCheep
{
    [Required]
    [StringLength(128, MinimumLength = 5)]
    public string Text { get; set; } = "";
    
}


