using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using Chirp.Core.Entities;
using Chirp.Core.Repository;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NuGet.Protocol.Core.Types;
using SQLitePCL;

namespace Chirp.Web.Pages;

public class PublicModel : PageModel
{
    private readonly ICheepService _service;
    private readonly ICheepRepository _cheeprepository;
    private readonly IAuthorRepository _authrepository;
    private readonly IValidator<CreateCheepDTO> _validator;

    public ICollection<CheepViewModel> Cheeps { get; set; }
    
   
    public PublicModel(ICheepService service, ICheepRepository cheeprepository, IAuthorRepository authrepository, IValidator<CreateCheepDTO> validator)
    {
        _service = service;
        _cheeprepository = cheeprepository;
        _authrepository = authrepository;
        _validator = validator; 
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

        await CreateCheep(cheep);

        RedirectToPage("/@User.Name");
    }
    
    public async Task CreateCheep(CreateCheepDTO newCheepDto)
    {
        var validationResult = await _validator.ValidateAsync(newCheepDto);
        if (!validationResult.IsValid)
        {
            Console.Out.WriteLine("hello"); //throw new ValidationException();
        }

        var author = _authrepository.GetAuthorByName(newCheepDto.Author);

        var entity = new CheepDTO()
        {
            Text = newCheepDto.Text,
            TimeStamp = DateTime.UtcNow,
            AuthorDto = author
        };
        
        _cheeprepository.AddCheep(entity);

    }

}

public class NewCheep
{
    [Required]
    [StringLength(128, MinimumLength = 5)]
    public string Text { get; set; } = "";
    
}


