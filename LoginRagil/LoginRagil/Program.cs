using LoginRagil.NewFolder;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using LoginRagil.Models;
using Microsoft.Extensions.DependencyInjection;
using LoginRagil.Servieces;

var builder = WebApplication.CreateBuilder(args);
//check for master
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<LoginDB>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Urls")));
builder.Services.AddScoped<UserManager<LUser>>();
builder.Services.AddScoped<SignInManager<LUser>>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "aaa",
        policy =>
        {
            policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
        });
});

builder.Services.AddIdentityCore<LUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
})
.AddEntityFrameworkStores<LoginDB>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
})
    .AddCookie(options =>
    {
        options.Cookie.Name = "user-shorty";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
    })
    .AddGoogle(googleoptions =>
    {
        googleoptions.ClientId = builder.Configuration["Authentication:Google:ClientId"]!;
        googleoptions.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"]!;
    });

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
