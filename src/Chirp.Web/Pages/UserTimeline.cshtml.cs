using Chirp.Core.Entities;
using Chirp.Core.Repository;
using Chirp.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Web.Pages;

public class UserTimelineModel : PageModel
{
    private readonly ICheepService _service;
    private readonly UserManager<Author> _userManager;
    private readonly IAuthorRepository _authorRepository;
    
    public Guid AuthorGuid;
    public ICollection<CheepViewModel> Cheeps { get; set; }
    public required Author user { get; set; }
    public required int currentPage;
    public required int totalPages { get; set; }
    
    public UserTimelineModel(ICheepService service, UserManager<Author> userManager, IAuthorRepository authorRepository)
    {
        _service = service;
        _userManager = userManager;
        _authorRepository = authorRepository;
    }

    public ActionResult OnGet(string author)
    {
        if(Request.Query.ContainsKey("page")){
            currentPage = int.Parse(Request.Query["page"]);
        } else{
            currentPage = 1;
        }

        user = _userManager.GetUserAsync(User).Result;
        Author timelineUser = _authorRepository.GetAuthorById(AuthorGuid);
        
        try
        {
            Cheeps = _service.GetCheepsFromAuthor(timelineUser.Id, currentPage);
        }
        catch (Exception e)
        {
            Cheeps = new List<CheepViewModel>();
        }
        
        totalPages = _authorRepository.GetPageCountByAuthor(timelineUser.Id);

        return Page();
    }
}
