using Chirp.Core.Repository;
using Chirp.Infrastructure;
using Chirp.Infrastructure.Repository;
using Chirp.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth; 
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("ApplicationDbContextConnection") ?? throw new InvalidOperationException("Connection string 'ApplicationDbContextConnection' not found.");

string currentDirectory = Directory.GetCurrentDirectory();
string dbPath;

if (Directory.Exists(Path.Combine(currentDirectory, "..", "Chirp.Infrastructure", "data")))
{
    dbPath = Path.Combine(currentDirectory, "..", "Chirp.Infrastructure", "data", "ChirpDBContext.db"); //Build directory
}
else 
{
    dbPath = Path.Combine(currentDirectory, "data", "ChirpDBContext.db"); //Publish directory
}

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddDbContext<ChirpDbContext>(options => 
    options.UseSqlite($"Data Source={dbPath}"));

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false )
    .AddEntityFrameworkStores<ChirpDbContext>();

builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
builder.Services.AddScoped<ICheepRepository, CheepRepository>();        
builder.Services.AddScoped<ICheepService, CheepService>();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(
	options =>
	{
		options.Cookie.Name = ".Chirp.Session";
    	options.IdleTimeout = TimeSpan.FromMinutes(10);
    	options.Cookie.HttpOnly = true;
    	options.Cookie.IsEssential = true;
	});

//Github oauth:
builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = "GitHub";
    })
    .AddCookie()
    .AddGitHub(o =>
    {
        o.ClientId = builder.Configuration["authentication:github:clientId"];
        o.ClientSecret = builder.Configuration["authentication:github:clientSecret"];
        o.CallbackPath = "/signin-github";
    });

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ChirpDbContext>();

    DbInitializer.SeedDatabase(context);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();
app.MapRazorPages();

Console.WriteLine("Main method is running");
app.Run();