using Superwish_FSD04_AppDevII_ASP.NET_Project.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddDbContext<ToysDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ToysDbContext>();
var userManager = builder.Services.BuildServiceProvider().GetService<UserManager<IdentityUser>>();
var roleManager = builder.Services.BuildServiceProvider().GetService<RoleManager<IdentityRole>>();

builder.Services.Configure<IdentityOptions>(options => {
    //Pasword Settings
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 5;

    //Lockout  settings
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    //User Settings
    options.User.AllowedUserNameCharacters = 
    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedAccount = false;
});

builder.Services.ConfigureApplicationCookie(options => {
    //cookie settings
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(25);

    options.LoginPath = "/Login";
    options.AccessDeniedPath ="/AccessDenied";
    options.SlidingExpiration = true;
});
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

app.UseAuthentication();
app.UseAuthorization();

//SeedUsersAndRoles(userManager, roleManager);

app.MapRazorPages();

/*private void SeedUsersAndRoles(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager) {
    string[] rolesNameList = new string[] {"User", "Admin"};
    foreach (string roleName in rolesNameList) {
        if(!roleManager.RoleExistsAsync(roleName).Result) {
            IdentityRole role = new IdentityRole();
            role.Name = roleName;
            IdentityResult result = roleManager.CreateAsync(role).Result;
        }
    }
    string adminEmail = "admin@admin.com";
    string adminPass = "Admin123!";
    if (userManager.FindByNameAsync(adminEmail).Result == null) {
        IdentityUser user = new IdentityUser();
        user.UserName = adminEmail;
        user.EmailConfirmed = true;
        IdentityResult result = userManager.CreateAsync(user, adminPass).Result;

        if (result.Succeeded){
            userManager.AddToRoleAsync(user, "Admin").Wait();
        }
    }
}*/

app.Run();
