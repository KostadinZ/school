# Fix localhost 7231 404

If the browser opens `https://localhost:7231` and shows `404 Not Found`, edit these files in your ASP.NET Core MVC project.

## 1. Edit `Program.cs`

Make sure your route section near the bottom of `Program.cs` looks like this:

```csharp
app.MapGet("/", () => Results.Redirect("/Events"));

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Events}/{action=Index}/{id?}");

app.MapFallbackToController("Index", "Events");
```

Why this matters:

- `app.MapGet("/", ...)` sends the plain localhost URL to the Events page.
- The default MVC route opens `EventsController.Index` when no controller/action is provided.
- The fallback route prevents a blank 404 when the browser opens a wrong path.

## 2. Add or edit `Controllers/HomeController.cs`

Create this file if it does not exist:

```csharp
using Microsoft.AspNetCore.Mvc;

namespace SchoolEventRegistration.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return RedirectToAction("Index", "Events");
    }

    public IActionResult Error()
    {
        return View();
    }
}
```

Why this matters:

- Some templates and browsers try to open `/Home` or `/Home/Index`.
- This controller redirects those URLs to your Events page.
- The `Error` action matches the exception handler in `Program.cs`.

## 3. Add or edit `Properties/launchSettings.json`

Create the `Properties` folder if needed, then add this file:

```json
{
  "$schema": "http://json.schemastore.org/launchsettings.json",
  "profiles": {
    "SchoolEventRegistration": {
      "commandName": "Project",
      "dotnetRunMessages": true,
      "launchBrowser": true,
      "launchUrl": "",
      "applicationUrl": "https://localhost:7231;http://localhost:5231",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    }
  }
}
```

Why this matters:

- It tells Visual Studio and `dotnet run` which local URLs to use.
- `launchUrl` is empty so the browser opens the root URL instead of a missing page.

## 4. Add `Views/Home/Error.cshtml`

Create the `Views/Home` folder if needed, then add this file:

```cshtml
@{
    ViewData["Title"] = "Error";
}

<div class="alert alert-danger shadow-sm" role="alert">
    <h1 class="h4">Something went wrong</h1>
    <p class="mb-0">Please return to the events page and try again.</p>
</div>

<a class="btn btn-primary" asp-controller="Events" asp-action="Index">Back to Events</a>
```

Why this matters:

- `Program.cs` uses `/Home/Error` outside Development mode.
- Without this view, production errors can produce another missing-page problem.

## 5. Run and open the correct URL

Use this command:

```bash
dotnet run --launch-profile SchoolEventRegistration
```

Then open one of these URLs:

- `https://localhost:7231`
- `http://localhost:5231`
- `https://localhost:7231/Events`

If HTTPS gives a certificate warning, try the HTTP URL first: `http://localhost:5231`.
