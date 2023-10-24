using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.VisualBasic;

namespace Chirp.Razor.Pages;

public class PublicModel : PageModel
{
    private readonly ICheepService _service;
    public ICollection<CheepViewModel> Cheeps { get; set; }

    public PublicModel(ICheepService service)
    {
        _service = service;
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
}
