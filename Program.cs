using Microsoft.EntityFrameworkCore;
using SchoolEventRegistration.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<SchoolEventContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("SchoolEventContext")
        ?? "Data Source=school-events.db"));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<SchoolEventContext>();
    context.Database.EnsureCreated();
    DbInitializer.Seed(context);
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapGet("/", () => Results.Redirect("/Events"));

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Events}/{action=Index}/{id?}");

app.MapFallbackToController("Index", "Events");

app.Run();
