using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using kasirResto.Data;
using kasirResto.Areas.Identity.Data;
using Microsoft.AspNetCore.Mvc.Authorization;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("kasirRestoContextConnection") ?? throw new InvalidOperationException("Connection string 'kasirRestoContextConnection' not found.");

builder.Services.AddDbContext<kasirRestoContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<kasirRestoUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<kasirRestoContext>();

// Add services to the container.
builder.Services.AddControllersWithViews();




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
app.UseAuthentication();;

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();
//otomatisLogin
app.MapControllers().RequireAuthorization();
app.Run();
