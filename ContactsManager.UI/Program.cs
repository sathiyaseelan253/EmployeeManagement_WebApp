using Services;
using Entities;
using Microsoft.EntityFrameworkCore;
using Repositories;
using RepositoryContracts;
using ServiceContracts;
using Services;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddRazorPages();
//builder.Services.AddScoped<ICountryService, CountriesService>();
//builder.Services.AddScoped<IPersonGetterService, PersonsGetterService>();
//add services into IoC container
builder.Services.AddScoped<ICountriesGetterService, CountriesGetterService>();
builder.Services.AddScoped<ICountriesAdderService, CountriesAdderService>();
builder.Services.AddScoped<ICountriesUploaderService, CountriesUploaderService>();
builder.Services.AddScoped<IPersonGetterService, PersonsGetterService>();
builder.Services.AddScoped<IPersonAdderService, PersonsAdderService>();
builder.Services.AddScoped<IPersonDeleterService, PersonsDeleterService>();
builder.Services.AddScoped<IPersonUpdaterService, PersonsUpdaterService>();
builder.Services.AddScoped<IPersonSorterService, PersonsSorterService>();
builder.Services.AddScoped<IPersonsRepository, PersonsRepository>();
builder.Services.AddScoped<ICountriesRepository, CountriesRepository>();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("defaultConnectionString"));
});
var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
Rotativa.AspNetCore.RotativaConfiguration.Setup("wwwroot", "Rotativa");
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Person}/{action=Index}/{id?}");

app.Run();
