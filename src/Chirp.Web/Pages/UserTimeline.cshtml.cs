using Chirp.Core.Entities;
using Chirp.Core.Repository;
using Chirp.Web;
using Chirp.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Razor.Pages;

public class UserTimelineModel : PageModel
{
    private readonly ICheepService _service;
    private readonly UserManager<Author> _userManager;
    private readonly IAuthorRepository _authorRepository;


    public ICollection<CheepViewModel> Cheeps { get; set; }
    public UserModel UserModel { get; set; }
    
    public required Author user { get; set; }
    public required int currentPage { get; set; }
    public required int totalPages { get; set; }



    public UserTimelineModel(ICheepService service, UserManager<Author> userManager, IAuthorRepository authorRepository)
    {
        _service = service;
        _userManager = userManager;
        _authorRepository = authorRepository;
    }

    public ActionResult OnGet(string author)
    {
        user = _userManager.GetUserAsync(User).Result;
        if (user == null)
        {
            return NotFound();
        }


        InitializeVariables(author);

        return Page();
    }


    public void InitializeVariables(string author)
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
        InitializeVariables(page, author);
    }

    public void InitializeVariables(int page, string author)
    {

        UserModel = new UserModel(user);

        //Get author object to allow the get page count method to be called on ID
        Author authorObject = _authorRepository.GetAuthorByName(author);
        totalPages = _authorRepository.GetPageCountByAuthor(authorObject.Id);
        currentPage = page;


        try
        {
            Cheeps = _service.GetCheepsFromAuthor(authorObject.Id, page);
        }
        catch (Exception e)
        {
            Cheeps = new List<CheepViewModel>();
        }
    }
}
