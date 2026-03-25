using System.Security.Claims;
using ApexCharts;
using Dashboard.Components;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
//builder.Services.AddApexCharts();

builder.Services.AddAuthentication("Cookies")
    .AddCookie("Cookies", options => {
        options.LoginPath = "/login";
        options.AccessDeniedPath = "/login";
        options.Cookie.HttpOnly = true;
        options.Cookie.SameSite = SameSiteMode.Strict;
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.SlidingExpiration = true;
    });
builder.Services.AddAuthorization();
builder.Services.AddCascadingAuthenticationState();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}

app.UseStaticFiles();
app.UseAntiforgery();
app.UseAuthentication();
app.UseAuthorization();

app.MapPost("/api/auth/login", async (HttpContext ctx, [FromForm] LoginRequest req) => {
    if (req.Username == "admin" && req.Password == "admin123") {
        var claims = new List<Claim> { new(ClaimTypes.Name, req.Username) };
        var principal = new ClaimsPrincipal(new ClaimsIdentity(claims, "Cookies"));
        await ctx.SignInAsync("Cookies", principal);
        ctx.Response.Redirect("/");
    } else {
        ctx.Response.Redirect("/login?error=1");
    }
}).DisableAntiforgery();

app.MapGet("/api/auth/logout", async (HttpContext ctx) => {
    await ctx.SignOutAsync("Cookies");
    ctx.Response.Redirect("/login");
});

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();

public record LoginRequest(string Username, string Password);
