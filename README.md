# School Event Registration

An ASP.NET Core MVC school assignment project where students can view school events and register for them.

## Features

- Event CRUD pages: list, create, edit, delete, and details.
- Student pages: add students and list all students.
- Registration pages: register a student for an event and view all registrations.
- Entity Framework Core relationship: `Student --- Registration --- Event`.
- Business rules:
  - Prevent registration when an event is full.
  - Prevent the same student from registering for the same event twice.
  - Show available seats for each event.
  - Search events by title.
- Bootstrap UI with cards, tables, progress bars, and validation messages.

## Run the project

```bash
dotnet restore
dotnet run --launch-profile SchoolEventRegistration
```

Then open `https://localhost:7231` or `http://localhost:5231`. The root URL redirects to the Events page, so you should not see a blank 404 page.

## Fix a localhost 7231 404

If `https://localhost:7231` opens but shows `404 Not Found`, check `Docs/FixLocalhost404.md`. It lists the exact edits for `Program.cs`, `HomeController.cs`, `launchSettings.json`, and the error view.

The application uses SQLite by default and creates `school-events.db` automatically on startup with sample events and students.
