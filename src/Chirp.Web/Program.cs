using System.Configuration;
using Chirp.Core.Entities;
using Chirp.Core.Repository;
using Chirp.Infrastructure;
using Chirp.Infrastructure.Repository;
using Chirp.Web;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("ApplicationDbContextConnection") ?? throw new InvalidOperationException("Connection string 'ApplicationDbContextConnection' not found.");

string currentDirectory = Directory.GetCurrentDirectory();

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddDbContext<ChirpDbContext>(options => 
    options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<Author>()
    .AddRoles<IdentityRole<Guid>>()
    .AddEntityFrameworkStores<ChirpDbContext>();

builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
builder.Services.AddScoped<IValidator<CreateCheep>, CheepCreateValidator>();
builder.Services.AddScoped<ICheepRepository, CheepRepository>();
builder.Services.AddScoped<ICheepService, CheepService>();
builder.Services.AddScoped<IReactionRepository, ReactionRepository>();

//builder.Services.AddDistributedMemoryCache();
//TODO databaseMigrate(context) lad være med at kalde ensure created før der bliver kaldt migrate

builder.Services.AddSession(
	options =>
	{
		options.Cookie.Name = ".Chirp.Session";
    	options.IdleTimeout = TimeSpan.FromMinutes(10);
    	options.Cookie.HttpOnly = false;
    	options.Cookie.IsEssential = true;
	});

//Github OAuth:
builder.Services.AddAuthentication()
    .AddCookie()
    .AddGitHub(o =>
    {
        o.ClientId = builder.Configuration["authenticationGithubClientId"];
        o.ClientSecret = builder.Configuration["authenticationGithubClientSecret"];
        o.CallbackPath = "/signin-github";
    });

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ChirpDbContext>();
    try {
        DbInitializer.SeedDatabase(context);
    } catch (Exception ex) {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred seeding the DB.");
    }
    
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();
app.MapRazorPages();
app.Run();