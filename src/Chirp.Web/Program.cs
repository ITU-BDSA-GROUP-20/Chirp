


        using Chirp.Infrastructure;
        using Chirp.Razor;
        using Microsoft.EntityFrameworkCore;

        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddRazorPages();
        
        builder.Services.AddDbContext<ChirpDbContext>(options => 
            options.UseSqlite("Data Source=../Chirp.Infrastructure/data/ChirpDBContext.db"));

        builder.Services.AddScoped<ICheepService, CheepService>();


        var app = builder.Build();

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

        app.MapRazorPages();

        Console.WriteLine("Main method is running");
        app.Run(); 

