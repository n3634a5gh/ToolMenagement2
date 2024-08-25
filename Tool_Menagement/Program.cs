using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Tool_Menagement.Models;
using Tool_Menagement.Repositories;
using Tool_Menagement.Interfaces;
using FluentValidation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ToolsBaseContext>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ITechnologieRepository, TechnologieRepository>();
builder.Services.AddScoped<IRejestracjaRepository, RejestracjaRepository>();
builder.Services.AddScoped<INarzedziaRepository, NarzedziaRepository>();

builder.Services.AddControllersWithViews();
builder.Services.AddControllersWithViews().AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<TechnologiaViewModel>());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
//app.MapRazorPages();

app.Run();
