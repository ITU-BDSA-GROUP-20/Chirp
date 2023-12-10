using System.ComponentModel.DataAnnotations;
using Chirp.Core.Entities;
using Chirp.Core.Repository;
using Chirp.Web.Models;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ValidationException = System.ComponentModel.DataAnnotations.ValidationException;

namespace Chirp.Web.Pages;

public abstract class TimeLineModel : PageModel
{
    protected readonly ICheepService _cheepService;
    protected readonly ICheepRepository _cheepRepository;
    protected readonly IAuthorRepository _authorRepository;
    protected readonly IReactionRepository _reactionRepository;
    protected readonly IValidator<CreateCheep> _cheepValidator;
    protected readonly UserManager<Author> _userManager;

    public int CurrentPage;
    public int TotalPages;

    public required Author SignedInUser;
    public required ICollection<CheepViewModel> Cheeps;

    public TimeLineModel(ICheepService cheepService,
        ICheepRepository cheepRepository,
        IAuthorRepository authorRepository,
        IReactionRepository reactionRepository,
        IValidator<CreateCheep> cheepValidator,
        UserManager<Author> userManager)
    {
        _cheepService = cheepService;
        _cheepRepository = cheepRepository;
        _authorRepository = authorRepository;
        _reactionRepository = reactionRepository;
        _cheepValidator = cheepValidator;
        _userManager = userManager;
    }
    
    public ActionResult OnGet()
    {
        InitializeVariables();
        InitializeCheepList();
        return Page();
    }

    protected abstract void InitializeCheepList();
    
    [BindProperty] public NewCheep Cheep { get; set; }

    public async Task<IActionResult> OnPostCreateCheep()
    {   
        
        if (!ModelState.IsValid)
        { 
            return Page();
        }
       
        var author = await _userManager.GetUserAsync(User);
        var cheep = new CreateCheep(author!, Cheep.Text!);

        await CreateCheep(cheep);
        
        return RedirectToPage("/UserTimeline", new { author = User.Identity?.Name });
        
    }
    
    public async Task CreateCheep(CreateCheep newCheep)
    {
        var validationResult = await _cheepValidator.ValidateAsync(newCheep);
         
        if (!validationResult.IsValid)
        {
            Console.WriteLine(validationResult);
            //Fatal exception
            throw new ValidationException("The Cheep must be between 5 and 160 characters.(CreateCheep)");
        }

        await _cheepRepository.AddCreateCheep(newCheep);
    }
  
    public async Task<IActionResult> OnPostReaction(Guid cheepId, ReactionType reactionType, int currentPage)
    {
        Author? author = await _userManager.GetUserAsync(User);
        if (await _reactionRepository.HasUserReacted(cheepId, author!.Id)) return Page();
        await _reactionRepository.AddReaction(reactionType, cheepId, author!.Id);
        InitializeVariables(currentPage);
        return Page();
    }
    public async Task<IActionResult> OnPostRemoveReaction(Guid cheepId, ReactionType reactionType, int currentPage)
    {
        Author? author = await _userManager.GetUserAsync(User);
        if (!await _reactionRepository.HasUserReacted(cheepId, author!.Id)) return Page();
        await _reactionRepository.RemoveReaction(reactionType, cheepId, author!.Id);
        InitializeVariables(currentPage);
        return Page();
    }
    
    [BindProperty] public string Author2FollowInput { get; set; }
    [BindProperty] public string currentPageFollowInput { get; set; }
    public async Task<IActionResult> OnPostFollow()
    {
        Guid authorFollowedId = Guid.Parse(Author2FollowInput);
        
        Author author = await _authorRepository.GetAuthorByIdAsync(_userManager.GetUserAsync(User).Result.Id);
        Author authorToFollow = await _authorRepository.GetAuthorByIdAsync(authorFollowedId);
        InitializeVariables(int.Parse(currentPageFollowInput));
        
        if (author == null) return Page();

        await _authorRepository.AddFollowing(author, authorToFollow);
        return Page();
    }

    [BindProperty] public string Author2UnfollowInput { get; set; }
    [BindProperty] public string currentPageUnfollowInput { get; set; }
    public async Task<IActionResult> OnPostUnfollow()
    {
        Guid authorUnfollowedId = Guid.Parse(Author2UnfollowInput);

        Author author = await _authorRepository.GetAuthorByIdAsync(_userManager.GetUserAsync(User).Result.Id);
        Author authorToUnfollow = await _authorRepository.GetAuthorByIdAsync(authorUnfollowedId);
        
        InitializeVariables(int.Parse(currentPageUnfollowInput));


        if (authorToUnfollow == null || author == null) return Page();

        await _authorRepository.RemoveFollowing(author!, authorToUnfollow);
        return Page();
    }

    public void InitializeVariables()
    {
        int page;
        if (Request.Query.ContainsKey("page"))
        {
            page = int.Parse(Request.Query["page"]!);
        }
        else
        {
            page = 1;
        }
        InitializeVariables(page);
    }

    public void InitializeVariables(int page)
    {
        SignedInUser = _userManager.GetUserAsync(User).Result;
        
        //Fetch user again to eagerly load followers
        SignedInUser = _authorRepository.GetAuthorById(SignedInUser.Id);
        TotalPages = _cheepRepository.GetPageCount();
        CurrentPage = page;
    }
    
    public class NewCheep
    {
        [Required]
        [StringLength(160, MinimumLength = 5, ErrorMessage = "The Cheep must be between 5 and 160 characters(NewCheep).")]
        public string? Text { get; set; }

        public override string ToString()
        {
            return Text;
        }
    }
}
