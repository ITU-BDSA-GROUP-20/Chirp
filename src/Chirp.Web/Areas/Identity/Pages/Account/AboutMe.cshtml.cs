﻿// Pages/AboutMe.cshtml.cs

using Chirp.Core.Entities;
using Chirp.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Web.Areas.Identity.Pages.Account;

public class AboutMeModel : PageModel
{
    private readonly UserManager<Author> _userManager;
    private readonly ICheepService _service;
    public UserModel UserModel { get; set; }
    public ICollection<CheepViewModel> Cheeps { get; set; }
    public ICollection<Author> Followers { get; set; }
    public ICollection<Author> Following { get; set; }
    // This is the user that the _CheepList is expected to find to create the cheeps
    public Author user { get; set; }

    public AboutMeModel(UserManager<Author> userManager, ICheepService service)
    {
        _userManager = userManager;
        _service = service;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        Console.WriteLine("It reached this point");
        // Fetch user information from the database
        user = await _userManager.GetUserAsync(User);

        if (user == null)
        {
            return NotFound();
        }

        // Create a UserModel based on the Author entity
        UserModel = new UserModel(user);
        
        // Retrieve the followers and following of the user
        try
        {
            Followers = _service.GetFollowers(UserModel.Id);
            Following = _service.GetFollowing(UserModel.Id);
        }
        catch (Exception e)
        {
            Followers = new List<Author>();
            Following = new List<Author>();
        }
        
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