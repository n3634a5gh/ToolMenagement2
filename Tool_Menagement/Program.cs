using Microsoft.EntityFrameworkCore;
using Tool_Menagement.Interfaces;
using Tool_Menagement.Models;
using Tool_Menagement.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ToolsBaseContext>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ITechnologieRepository, TechnologieRepository>();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
