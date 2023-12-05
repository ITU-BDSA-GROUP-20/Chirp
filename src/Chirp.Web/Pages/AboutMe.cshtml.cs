// Pages/AboutMe.cshtml.cs

using Chirp.Core.Entities;
using Chirp.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Web.Pages;

public class AboutMeModel : PageModel
{
    private readonly UserManager<Author> _userManager;
    private readonly ICheepService _service;
    public UserModel UserModel { get; set; }
    public ICollection<CheepViewModel> Cheeps { get; set; }

    public AboutMeModel(UserManager<Author> userManager, ICheepService service)
    {
        _userManager = userManager;
        _service = service;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        // Fetch user information from the database
        var authorEntity = await _userManager.GetUserAsync(User);

        if (authorEntity == null)
        {
            return NotFound();
        }

        // Create a UserModel based on the Author entity
        UserModel = new UserModel(authorEntity);
        
        int page;
        if(Request.Query.ContainsKey("page")){
            page = int.Parse(Request.Query["page"]);
        } else{
            page = 1;
        }
        
        try
        {
            Cheeps = _service.GetCheepsFromAuthor(UserModel.Username, 1);
        }
        catch (Exception e)
        {
            Cheeps = new List<CheepViewModel>();
        }

        return Page();
    }
}