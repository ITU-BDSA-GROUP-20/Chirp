﻿using System.ComponentModel.DataAnnotations;
using Chirp.Core.Entities;
using Chirp.Core.Repository;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ValidationException = FluentValidation.ValidationException;

namespace Chirp.Web.Pages;

public class PublicModel : PageModel
{
    private readonly ICheepService _service;
    private readonly ICheepRepository _cheeprepository;
    private readonly IAuthorRepository _authrepository;
    private readonly IValidator<CreateCheep> _validator;
    
    private readonly UserManager<Author> _userManager;
    public required ICollection<CheepViewModel> Cheeps { get; set; }
   


    public PublicModel(ICheepService service, ICheepRepository cheeprepository, IAuthorRepository authrepository, IValidator<CreateCheep> validator , UserManager<Author> userManager)
    {
        _service = service;
        _cheeprepository = cheeprepository;
        _authrepository = authrepository;
        _validator = validator;
        _userManager = userManager;
    }

    public ActionResult OnGet()
    {
        int page;
       if(Request.Query.ContainsKey("page")){
            page = int.Parse(Request.Query["page"]! );
       } else{
            page = 1;
       }
        Cheeps = _service.GetCheeps(page) ?? new List<CheepViewModel>();
        return Page();
    }
    
   
}



