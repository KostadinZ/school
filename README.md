# School Voting System (ASP.NET Core MVC + Entity Framework Core)

A complete ASP.NET Core MVC voting application with Entity Framework Core and SQLite.

## Features
- Multiple voting positions in one election (e.g., President, School President, Vice President).
- Multiple candidate options for each position.
- One vote per voter per position (duplicate prevention).
- Live result page with vote totals per position and candidate.
- Seeded sample election data.

## Tech stack
- ASP.NET Core MVC (latest target in this sample: **.NET 9**)
- Entity Framework Core 9
- SQLite

## Run locally
1. Install the latest .NET SDK (9+).
2. From the project root:
   ```bash
   dotnet restore
   dotnet run
   ```
3. Open the launch URL (typically `https://localhost:7242`).

On first run, the app automatically creates the SQLite database and seeds sample data.

## Project structure
- `Controllers/` - MVC controllers (`HomeController`, `VotesController`)
- `Models/` - EF Core entities (`Election`, `VotingPosition`, `CandidateOption`, `Vote`)
- `Data/` - DbContext and data seeding
- `ViewModels/` - Input and results view models
- `Views/` - Razor views and layout
- `wwwroot/` - static CSS assets
