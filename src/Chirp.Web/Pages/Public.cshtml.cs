﻿using System.ComponentModel.DataAnnotations;
using Chirp.Core.Entities;
using Chirp.Core.Repository;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SQLitePCL;
using ValidationException = FluentValidation.ValidationException;

namespace Chirp.Web.Pages;

public class PublicModel : PageModel
{
    private readonly ICheepService _service;
    private readonly ICheepRepository _cheepRepository;
    private readonly IAuthorRepository _autherRepository;
    private readonly IValidator<CreateCheep> _validator;
    public required Author user { get; set; }
    
    private readonly UserManager<Author> _userManager;
    
    public required ICollection<CheepViewModel> Cheeps { get; set; }
   


    public PublicModel(ICheepService service, ICheepRepository cheeprepository, IAuthorRepository autherRepository, IValidator<CreateCheep> validator , UserManager<Author> userManager)
    {
        _service = service;
        _cheepRepository = cheeprepository;
        _autherRepository = autherRepository;
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

        user = _autherRepository.GetAuthorByName(_userManager.GetUserAsync(User).Result.UserName);
        
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
        
        return RedirectToPage("/UserTimeline", new { author = User.Identity?.Name });;
        
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
        Guid authorId = user.Id;
        Guid authorFollowedId = Guid.Parse(Author2FollowInput);
        
        Author author = await _autherRepository.GetAuthorByIdAsync(authorId);
        Author authorToFollow = await _autherRepository.GetAuthorByIdAsync(authorFollowedId);

        if (author == null) return Page();

        await _autherRepository.AddFollowing(author, authorToFollow);
        return Page();
    }

    [BindProperty] public string Author2UnfollowInput { get; set; }
    public async Task<IActionResult> OnPostUnfollow()
    {
        Guid authorId = user.Id;
        Guid authorFollowedId = Guid.Parse(Author2UnfollowInput);

        Author author = await _autherRepository.GetAuthorByIdAsync(authorId);
        Author authorToUnfollow = await _autherRepository.GetAuthorByIdAsync(authorFollowedId);

        if (authorToUnfollow == null || author == null) return Page();

        await _autherRepository.RemoveFollowing(author!, authorToUnfollow);
        return Page();
    }
   
   
}

public class NewCheep
{
    [Required]
    [StringLength(160, MinimumLength = 5, ErrorMessage = "The Cheep must be between 5 and 160 characters(NewCheep).")]
    public string? Text { get; set; }

}



