using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MusicApp.Areas.Identity.Data;
using MusicApp.Data;
using MusicApp.Hubs; 
using System;
var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("MusicConn") ?? throw new InvalidOperationException("Connection string 'MusicAppIdentityContextConnection' not found.");


builder.Services.AddDbContext<MusicAppIdentityContext>(options =>
{
    options.UseSqlServer(connectionString);

});
builder.Services.AddDbContext<MusicDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<MusicAppUser>(options => options.SignIn.RequireConfirmedAccount = false).AddEntityFrameworkStores<MusicAppIdentityContext>();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSignalR();
 

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
app.MapRazorPages();
app.MapHub<ChatHub>("/chatHub");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
