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

    public AboutMeModel(UserManager<Author> userManager)
    {
        _userManager = userManager;
    }

    public UserModel UserModel { get; set; }

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

        return Page();
    }
}