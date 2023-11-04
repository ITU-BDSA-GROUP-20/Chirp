using System.Net.Mime;
using Chirp.Core.Entities;
using Chirp.Core.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NuGet.Protocol.Core.Types;
using SQLitePCL;

namespace Chirp.Web.Pages;

public class PublicModel : PageModel
{
    private readonly ICheepService _service;
    private readonly ICheepRepository _repository;

    public ICollection<CheepViewModel> Cheeps { get; set; }
    
   
    public PublicModel(ICheepService service, ICheepRepository repository)
    {
        _service = service;
        _repository = repository;
    }

    public ActionResult OnGet()
    {
        int page;
       if(Request.Query.ContainsKey("page")){
            page = int.Parse(Request.Query["page"]);
       } else{
            page = 1;
       }
        Cheeps = _service.GetCheeps(page);
        return Page();
    }
    
    [BindProperty]
    public NewCheep NewCheep { get; set; }

    public async Task OnPost()
    {
        var cheep = new CreateCheepDTO(User.Identity.Name, NewCheep.Text);

        await _repository.CreateCheep(cheep);

        RedirectToPage("/@User.Name");
    }


}

public class NewCheep
{
    public string Text { get; set; } = "";
    
}


