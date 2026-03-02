using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApi.Entities;
using WebApi.Interfaces;
using WebApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages(options =>
{
    // require authenticated users for product and user areas by default
    options.Conventions.AuthorizeFolder("/Products");
    options.Conventions.AuthorizeFolder("/Users");
    // allow anyone to register
    options.Conventions.AllowAnonymousToPage("/Auth/Register");
});
builder.Services.AddScoped<IProductService, ProductService>();

builder.Services.AddDbContext<ApplicationDbContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services
    .AddIdentity<ApplicationUser, IdentityRole>(options =>
    {
        options.User.RequireUniqueEmail = true;

        options.Password.RequiredLength = 8;
        options.Password.RequireDigit = true;
        options.Password.RequireUppercase = false;
        options.Password.RequireNonAlphanumeric = false;

        options.Lockout.MaxFailedAccessAttempts = 5;
        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

// make sure the signin/redirect paths line up with our RazorPages layout
builder.Services.ConfigureApplicationCookie(options =>
{
    // the login page lives under /Auth/Login.cshtml in this project
    options.LoginPath = "/Auth/Login";
    options.LogoutPath = "/Auth/Logout"; // not used yet but set for completeness
    options.AccessDeniedPath = "/Auth/AccessDenied"; // redirect here when user lacks role
});

var app = builder.Build();

// seed roles and default super‑admin user on startup
try
{
    await using var scope = app.Services.CreateAsyncScope();
    var services = scope.ServiceProvider;
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    await WebApi.Seeds.DefaultRoles.SeedRoles(roleManager);

    // create a default super‑admin if it doesn't exist
    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
    const string superAdminEmail = "superadmin@localhost";
    const string superAdminPassword = "SuperAdmin123!";
    var super = await userManager.FindByEmailAsync(superAdminEmail);
    if (super == null)
    {
        super = new ApplicationUser
        {
            UserName = superAdminEmail,
            Email = superAdminEmail,
            FullName = "Super Administrator"
        };
        var createResult = await userManager.CreateAsync(super, superAdminPassword);
        if (createResult.Succeeded)
        {
            await userManager.AddToRoleAsync(super, "SuperAdmin");
        }
    }
}
catch (Exception ex)
{
    // logging in a minimal project; failing to seed should not stop the app
    Console.WriteLine("Error seeding roles: " + ex.Message);
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();
