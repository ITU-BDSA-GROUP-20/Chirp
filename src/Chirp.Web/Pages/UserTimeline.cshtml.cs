using Chirp.Core.Entities;
using Chirp.Core.Repository;
using Chirp.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.CodeAnalysis.Elfie.Serialization;

namespace Chirp.Razor.Pages;

public class UserTimelineModel : PageModel
{
    private readonly ICheepService _service;
    private readonly UserManager<Author> _userManager;
    
    public ICollection<CheepViewModel> Cheeps { get; set; }
    public Guid AuthorId;
    
    public required Author user { get; set; }

    public UserTimelineModel(ICheepService service, UserManager<Author> userManager)
    {
        _service = service;
        _userManager = userManager;
    }

    public ActionResult OnGet(Guid authorId)
    {
        int page;
        if(Request.Query.ContainsKey("page")){
            page = int.Parse(Request.Query["page"]);
        } else{
            page = 1;
        }

        AuthorId = authorId;
        
        try
        {
            Cheeps = _service.GetCheepsFromAuthor(AuthorId, page);
        }
        catch (Exception e)
        {
            Cheeps = new List<CheepViewModel>();
        }

        user = _userManager.GetUserAsync(User).Result;

        return Page();
    }
}
