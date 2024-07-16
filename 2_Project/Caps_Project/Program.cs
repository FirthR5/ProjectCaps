using Caps_Project.DTOs;
using Caps_Project.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
//builder.Services.AddDbContext<DbCapsContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DevDB")));
builder.Services.AddDbContext<DbCapsContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DevDB")));
// Agregar Automappers
builder.Services.AddAutoMapper(typeof(MappingConfig));

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddAuthentication(
    CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(
    option =>
    {
        option.LoginPath = "/Home/Login";
        option.AccessDeniedPath = "/Home/Login";
        option.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    }
);



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization(); 

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
