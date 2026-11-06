# SleepyDuck

Sequential Scenario Flashcards: spaced repetition (SM-2) applied to multi-step Microsoft 365 admin troubleshooting. Each card is an incident you work through step by step. One wrong action fails the card.

| Folder      | Stack                                                                    |
| ----------- | ------------------------------------------------------------------------ |
| `backend/`  | .NET 8 Web API, Entity Framework Core, PostgreSQL                        |
| `frontend/` | Angular 21 (standalone components, signals, control flow), Tailwind CSS  |

## Running it

You need Docker, the .NET SDK, and Node.js 22. The API targets `net8.0` and rolls forward to a newer runtime if .NET 8 isn't installed.

```sh
# 1. Database
docker compose up -d

# 2. API on http://localhost:5080. Migrations and seed data are applied on startup in Development.
cd backend/SleepyDuck.Api
dotnet run

# 3. App on http://localhost:4200. /api requests are proxied to the backend.
cd frontend
npm install
npm start
```

Backend tests: `dotnet test` in `backend/`.

To reset all progress: `docker compose down -v`, then start again.

## API

| Method | Route                                  | Purpose                                                   |
| ------ | -------------------------------------- | --------------------------------------------------------- |
| GET    | `/api/dashboard/stats`                 | Dashboard counts, overall and per category                |
| GET    | `/api/scenarios`                       | All cards with their progress                             |
| GET    | `/api/scenarios/{id}`                  | One card                                                  |
| GET    | `/api/scenarios/queue`                 | Study queue. Optional `category` and `newLimit` (default 10) |
| POST   | `/api/scenarios/{id}/steps/{stepId}/answer` | Check an answer. A wrong answer applies "Again"      |
| POST   | `/api/scenarios/{id}/review`           | Apply a rating: `Again`, `Hard`, `Good`, or `Easy`        |

Answers are checked on the server, and card payloads never include which option is correct.

## Scheduling

| Rating | New card | Second review | After that                |
| ------ | -------- | ------------- | ------------------------- |
| Again  | Today    | Today         | Today, interval resets    |
| Hard   | 1 day    | Interval × 1.2 | Interval × 1.2           |
| Good   | 2 days   | 6 days        | Interval × ease           |
| Easy   | 4 days   | 6+ days × 1.3 | Interval × ease × 1.3     |

Ease starts at 2.5 and never falls below 1.3. Intervals are capped at 365 days. A card counts as Mastered at an interval of 21 days or more. "Today" is measured in UTC.

## Adding scenarios

Add to `Scenarios` in `backend/SleepyDuck.Api/Data/SeedData.cs`, then create a migration:

```sh
cd backend
dotnet ef migrations add AddMoreScenarios --project SleepyDuck.Api
```

## Design system

The "Warm Dark Mode Oasis" palette lives in `frontend/tailwind.config.js` under the `oasis` color namespace.

| Role                          | Token          | Hex       |
| ----------------------------- | -------------- | --------- |
| Deep background (dark)        | `oasis-deep`   | `#1E1E2F` |
| Card background (dark)        | `oasis-card`   | `#2B2B3E` |
| Accent and success            | `oasis-olive`  | `#4E5B31` |
| Main background (light)       | `oasis-sand`   | `#F0EDE8` |
| Borders (light)               | `oasis-mist`   | `#D1D1D1` |

The config also defines supporting tones. `#4E5B31` is too dark to read as text on `#1E1E2F`, so accent text in dark mode uses `oasis-sage` instead. Olive stays in use as a fill behind light text.

Dark mode is class-based and dark is the default. The toggle in the header adds or removes `dark` on `<html>` and saves the choice in `localStorage`. Tailwind 4 reads the JavaScript config through the `@config` line in `src/styles.css`.

## Project structure

```text
SleepyDuck/
├── backend/
│   ├── SleepyDuck.Api/
│   │   ├── Controllers/                                   # REST endpoints
│   │   │   ├── DashboardController.cs
│   │   │   └── ScenariosController.cs
│   │   ├── Data/
│   │   │   ├── AppDbContext.cs                            # EF Core model and relationships
│   │   │   └── SeedData.cs                                # Microsoft 365 scenario deck
│   │   ├── Dtos/
│   │   │   └── ApiDtos.cs                                 # API contracts, mirrored in api.models.ts
│   │   ├── Migrations/                                    # Generated schema and seed migrations
│   │   ├── Models/                                        # Entities and enums
│   │   │   ├── Distractor.cs
│   │   │   ├── Enums.cs
│   │   │   ├── ScenarioCard.cs
│   │   │   ├── SequentialStep.cs
│   │   │   └── UserProgress.cs
│   │   ├── Properties/
│   │   │   └── launchSettings.json
│   │   ├── Services/
│   │   │   ├── Sm2Scheduler.cs                            # SM-2 scheduling rules
│   │   │   └── StudyService.cs                            # Queue, answer checking, progress
│   │   ├── appsettings.json
│   │   ├── Program.cs
│   │   └── SleepyDuck.Api.csproj
│   ├── SleepyDuck.Api.Tests/                              # xUnit tests
│   │   ├── SeedDataTests.cs
│   │   ├── SleepyDuck.Api.Tests.csproj
│   │   └── Sm2SchedulerTests.cs
│   └── SleepyDuck.sln
├── frontend/
│   ├── .vscode/
│   │   ├── extensions.json
│   │   ├── launch.json
│   │   ├── mcp.json
│   │   └── tasks.json
│   ├── public/
│   │   └── favicon.svg
│   ├── src/
│   │   ├── app/
│   │   │   ├── core/
│   │   │   │   ├── models/
│   │   │   │   │   ├── api.models.ts                      # Interfaces matching the C# DTOs
│   │   │   │   │   └── labels.ts                          # Display labels and interval formatting
│   │   │   │   └── services/
│   │   │   │       ├── dashboard.service.ts
│   │   │   │       ├── scenario.service.ts
│   │   │   │       └── theme.service.ts                   # Light/dark state and persistence
│   │   │   ├── features/
│   │   │   │   ├── dashboard/                             # Stats and topic progress
│   │   │   │   │   ├── dashboard.html
│   │   │   │   │   └── dashboard.ts
│   │   │   │   └── study/                                 # Sequential scenario flow
│   │   │   │       ├── study-session.html
│   │   │   │       └── study-session.ts
│   │   │   ├── shared/
│   │   │   │   └── auto-height.ts                         # Smooth height changes between views
│   │   │   ├── app.config.ts
│   │   │   ├── app.routes.ts
│   │   │   └── app.ts                                     # App shell and theme toggle
│   │   ├── index.html                                     # Restores the saved theme before first paint
│   │   ├── main.ts
│   │   └── styles.css                                     # Design system: tokens, components, animations
│   ├── .editorconfig
│   ├── .gitignore
│   ├── .postcssrc.json
│   ├── angular.json
│   ├── package-lock.json
│   ├── package.json
│   ├── proxy.conf.json                                    # Forwards /api to the backend in development
│   ├── README.md
│   ├── tailwind.config.js                                 # Oasis palette, fonts, class-based dark mode
│   ├── tsconfig.app.json
│   ├── tsconfig.json
│   └── tsconfig.spec.json
├── .gitignore
├── docker-compose.yml                                     # Local PostgreSQL
├── README.md
└── rules.md                                               # Original project brief
```
