# School Event Registration

A simple ASP.NET Core MVC project where students can register for school events.

## What it includes

- Events: list, create, edit, delete, and details.
- Students: add and list students.
- Registrations: register a student for an event and list registrations.
- Simple rules:
  - Do not register for a full event.
  - Do not register the same student for the same event twice.
  - Show seats left for each event.
  - Search events by title.

## Run the project

```bash
dotnet restore
dotnet run --launch-profile SchoolEventRegistration
```

Then open:

- `https://localhost:7231`
- `http://localhost:5231`

The app uses a local SQLite database named `school-events.db`.
