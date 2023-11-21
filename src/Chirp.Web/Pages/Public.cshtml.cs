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
    private readonly ICheepRepository _cheepRepository;
    private readonly IAuthorRepository _authrepository;
    private readonly IValidator<CreateCheep> _validator;
    
    private readonly UserManager<Author> _userManager;
    
    public required ICollection<CheepViewModel> Cheeps { get; set; }
   


    public PublicModel(ICheepService service, ICheepRepository cheeprepository, IAuthorRepository authrepository, IValidator<CreateCheep> validator , UserManager<Author> userManager)
    {
        _service = service;
        _cheepRepository = cheeprepository;
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
    
    [BindProperty] public NewCheep NewCheep { get; set; }

    public async Task<IActionResult> OnPostCreateCheep()
    {   
        
        if (!ModelState.IsValid)
        {
           return Page();
        }
       
        var author = await _userManager.GetUserAsync(User);
        var cheep = new CreateCheep(author!, NewCheep.Text);

        await CreateCheep(cheep);
        
        return RedirectToPage("/UserTimeline", new { author = User.Identity.Name });;
        
    }
    
    public async Task CreateCheep(CreateCheep newCheep)
    {
         var validationResult = await _validator.ValidateAsync(newCheep);
         
         if (!validationResult.IsValid)
         {
             Console.WriteLine(validationResult);
             throw new ValidationException("The message must be between 5 and 160 characters.");
         }

         await _cheepRepository.AddCheep(newCheep);
    }
   
}

public record NewCheep
{
    [Required] 
    public string? Text { get; set; }
}



