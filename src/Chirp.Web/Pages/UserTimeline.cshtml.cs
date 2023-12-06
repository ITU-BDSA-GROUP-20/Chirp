using Chirp.Core.Entities;
using Chirp.Core.Repository;
using Chirp.Web;
using Chirp.Web.Models;
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
    public UserModel UserModel { get; set; }
    
    public required Author user { get; set; }

    public UserTimelineModel(ICheepService service, UserManager<Author> userManager)
    {
        _service = service;
        _userManager = userManager;
    }

    public ActionResult OnGet()
    {
        user = _userManager.GetUserAsync(User).Result;
        if (user == null)
        {
            return NotFound();
        }
        
        UserModel = new UserModel(user);
        
        int page;
        if(Request.Query.ContainsKey("page")){
            page = int.Parse(Request.Query["page"]);
        } else{
            page = 1;
        }
        
        try
        {
            Cheeps = _service.GetCheepsFromAuthor(UserModel.Id, page);
        }
        catch (Exception e)
        {
            Cheeps = new List<CheepViewModel>();
        }

        

        return Page();
    }
}
