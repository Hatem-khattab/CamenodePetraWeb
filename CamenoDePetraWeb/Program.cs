using CamenoDePetraWeb.Models.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);




builder.Services.AddControllersWithViews()
    .AddViewLocalization()
    .AddDataAnnotationsLocalization();


#region DbContext & Identity
builder.Services.AddDbContext<ERPDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<ERPDbContext>()
    .AddDefaultTokenProviders();
#endregion

var app = builder.Build();

#region Middleware Pipeline

if (!app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.Use(async (context, next) =>
{
    var supportedLanguages = new[] { "en", "fr", "es","pt"};
    string lang;

    if (context.Request.Cookies.TryGetValue("Language", out lang)
        && supportedLanguages.Contains(lang))
    {
        var culture = new CultureInfo(lang);

        CultureInfo.CurrentCulture = culture;
        CultureInfo.CurrentUICulture = culture;
    }
    else
    {
        var culture = new CultureInfo("en");

        CultureInfo.CurrentCulture = culture;
        CultureInfo.CurrentUICulture = culture;
    }

    await next();
});


app.UseAuthentication();
app.UseAuthorization();

#endregion

#region Routes
app.MapControllerRoute(
    name: "default",
    pattern: "{language=en}/{controller=Review}/{action=Index}/{id?}");
#endregion
app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = ctx =>
    {
        // كل الملفات الثابتة يتم حفظها في المتصفح لمدة 30 يوم
        ctx.Context.Response.Headers.Append("Cache-Control", "public,max-age=2592000");
    }
});


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await CreateSuperAdmin(services); // ← هذا السطر ينفذ إنشاء الـ Admin
    await SeedReview(services);
}

app.Run();
async Task SeedReview(IServiceProvider serviceProvider)
{
    var db = serviceProvider.GetRequiredService<ERPDbContext>();

    // إذا ما في أي ريفيو موجود
    if (!db.Reviews.Any())
    {
        var review = new Review
        {
            Name = "Hatem Raslan",
            Email = "HKDR@Gmail.com",
            Message = "we had so much fun time .",
            Rating = 5,
            CreatedAt = DateTime.UtcNow
        };

        db.Reviews.Add(review);
        await db.SaveChangesAsync();
    }
}
async Task CreateSuperAdmin(IServiceProvider serviceProvider)
{
    var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
    var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    string roleName = "SuperAdmin";
    string userName = "superadmin";
    string email = "Ahed@Cameno.com";
    string password = "Ahdim@1987"; // كلمة السر اللي تختارها

    // إنشاء Role إذا مش موجود
    if (!await roleManager.RoleExistsAsync(roleName))
    {
        await roleManager.CreateAsync(new IdentityRole(roleName));
    }

    // إنشاء User إذا مش موجود
    var user = await userManager.FindByNameAsync(userName);
    if (user == null)
    {
        user = new IdentityUser
        {
            UserName = userName,
            Email = email,
            EmailConfirmed = true
        };

        await userManager.CreateAsync(user, password);
        await userManager.AddToRoleAsync(user, roleName);
    }
}