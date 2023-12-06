using System.ComponentModel.DataAnnotations;
using Chirp.Core.Entities;
using Chirp.Core.Repository;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ValidationException = FluentValidation.ValidationException;

namespace Chirp.Web.Pages;

public class PublicModel : PageModel
{
    private readonly ICheepService _service;
    private readonly ICheepRepository _cheepRepository;
    private readonly IAuthorRepository _authrepository;
    private readonly IValidator<CreateCheep> _validator;
    public required Author user { get; set; }
    private readonly UserManager<Author> _userManager;
    public required ICollection<CheepViewModel> Cheeps { get; set; }
    public required int totalPages { get; set; }
   


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

        user = _userManager.GetUserAsync(User).Result;
        totalPages = _cheepRepository.GetPageCount();

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
        var cheep = new CreateCheep(author!, NewCheep.Text!);

        await CreateCheep(cheep);
        
        return RedirectToPage("/UserTimeline", new { author = User.Identity?.Name });
        
    }
    
    public async Task CreateCheep(CreateCheep newCheep)
    {
         var validationResult = await _validator.ValidateAsync(newCheep);
         
          if (!validationResult.IsValid)
          {
              Console.WriteLine(validationResult);
              //Fatal exception
              throw new ValidationException("The Cheep must be between 5 and 160 characters.(CreateCheep)");
         }

         await _cheepRepository.AddCreateCheep(newCheep);
    }
    
    [BindProperty] public string Author2FollowInput { get; set; }
    public async Task<IActionResult> OnPostFollow()
    {
        Guid author2FollowId = Guid.Parse(Author2FollowInput);
        Author? author = await _userManager.GetUserAsync(User);
        Author authorToFollow = await _authrepository.GetAuthorByIdAsync(author2FollowId);


        if (author == null) return Page();
        if (author.Following.Contains(authorToFollow)) return Page();


        await _authrepository.AddFollowing(author, authorToFollow);
        return Page();
    }

    [BindProperty] public string Author2UnfollowInput { get; set; }
    public async Task<IActionResult> OnPostUnfollow()
    {
        Guid author2UnfollowId = Guid.Parse(Author2UnfollowInput);
        Author author = await _userManager.GetUserAsync(User);
        Author authorToUnfollow = await _authrepository.GetAuthorByIdAsync(author2UnfollowId);

        if (authorToUnfollow == null || author == null) return Page();
        if (!author.Following.Contains(authorToUnfollow)) return Page();

        await _authrepository.RemoveFollowing(author!, authorToUnfollow);
        return Page();
    }


    [BindProperty] public string NewPage { get; set; }
    public IActionResult OnPostGoToPage()
    {
        int page = int.Parse(NewPage);

        if (page < 1) page = 1;
        Cheeps = _service.GetCheeps(page);
        user = _userManager.GetUserAsync(User).Result;

        totalPages = _cheepRepository.GetPageCount();

        

        return Page();
    }
   
   
}

public class NewCheep
{
    [Required]
    [StringLength(160, MinimumLength = 5, ErrorMessage = "The Cheep must be between 5 and 160 characters(NewCheep).")]
    public string? Text { get; set; }

}



