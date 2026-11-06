# SleepyDuck

Sequential Scenario Flashcards: spaced repetition (SM-2) applied to multi-step IT support troubleshooting, tuned for interview cramming. Topics cover Microsoft 365 administration, VOIP support, network troubleshooting, and Mac network issues. Each card is an incident you work through step by step. One wrong action fails the card.

| Folder      | Stack                                                                    |
| ----------- | ------------------------------------------------------------------------ |
| `backend/`  | .NET 8 Web API, Entity Framework Core, PostgreSQL                        |
| `frontend/` | Angular 21 (standalone components, signals, control flow), Tailwind CSS  |

## Running it

Setup guides: [Windows](#how-to-run-this-in-windows) and [Ubuntu](#how-to-run-this-in-ubuntu). Both need Docker, the .NET SDK, and Node.js 22. The app runs at http://localhost:4200 and sends `/api` requests to the backend on http://localhost:5080.

These commands work the same on both systems:

| Task                 | Command                                                   |
| -------------------- | --------------------------------------------------------- |
| Run the backend tests | `dotnet test`, from `backend/`. See [test.md](test.md)    |
| Reset all progress   | `docker compose down -v`, then start again                |

## How to run this in Windows

These steps were written for Windows 11, using PowerShell.

### Required software

| Software            | winget package           | Used for                               |
| ------------------- | ------------------------ | -------------------------------------- |
| Docker Desktop      | `Docker.DockerDesktop`   | Runs the PostgreSQL container          |
| .NET 8 SDK          | `Microsoft.DotNet.SDK.8` | Builds and runs the API                |
| Node.js 22          | `OpenJS.NodeJS.22`       | Builds and serves the Angular frontend |

A newer .NET SDK also works. The API targets `net8.0` and rolls forward to a newer runtime if .NET 8 isn't installed.

### 1. Install the software

```powershell
winget install --id Docker.DockerDesktop --exact
winget install --id Microsoft.DotNet.SDK.8 --exact
winget install --id OpenJS.NodeJS.22 --exact
```

Close and reopen PowerShell so it picks up the new commands. If you get a "running scripts is disabled" error from `npm` later, run this once:

```powershell
Set-ExecutionPolicy -Scope CurrentUser RemoteSigned
```

Check the versions:

```powershell
docker compose version
dotnet --version
node --version     # v22.x
```

### 2. Start PostgreSQL

Open Docker Desktop and wait until it says the engine is running. Then, from the project root:

```powershell
docker compose up -d --wait
```

### 3. Run the API

In a new terminal, from the project root. Migrations and seed data are applied on startup. The API listens on http://localhost:5080.

```powershell
cd backend\SleepyDuck.Api
dotnet run
```

### 4. Start the frontend

In another terminal, from the project root:

```powershell
cd frontend
npm ci
npm start
```

Open http://localhost:4200.

### Stopping it

Press `Ctrl+C` in the API and frontend terminals, then run `docker compose down` from the project root. Your progress stays in a Docker volume until you add `-v`.

## How to run this in Ubuntu

These steps were written for Ubuntu 24.04 LTS.

### Required packages

| Package                              | Used for                                   |
| ------------------------------------ | ------------------------------------------ |
| `docker.io`, `docker-compose-v2`     | Runs the PostgreSQL container              |
| `dotnet-sdk-8.0`                     | Builds and runs the API                    |
| `nodejs` 22, from NodeSource         | Builds and serves the Angular frontend     |
| `git`, `curl`, `ca-certificates`     | Getting the code and the NodeSource script |
| `tzdata`                             | Provides the `America/Guatemala` time zone |

### 1. Install the packages

```sh
sudo apt update
sudo apt install -y git curl ca-certificates tzdata docker.io docker-compose-v2 dotnet-sdk-8.0

# Ubuntu's own nodejs package is older than Angular 21 needs, so use NodeSource.
curl -fsSL https://deb.nodesource.com/setup_22.x | sudo -E bash -
sudo apt install -y nodejs

# Run Docker without sudo. Log out and back in for this to take effect.
sudo systemctl enable --now docker
sudo usermod -aG docker "$USER"
```

Check the versions:

```sh
docker compose version
dotnet --version   # 8.0.x
node --version     # v22.x
```

### 2. Start PostgreSQL

From the project root:

```sh
docker compose up -d --wait
```

### 3. Run the API

In a new terminal, from the project root. Migrations and seed data are applied on startup. The API listens on http://localhost:5080.

```sh
cd backend/SleepyDuck.Api
dotnet run
```

### 4. Start the frontend

In another terminal, from the project root:

```sh
cd frontend
npm ci
npm start
```

Open http://localhost:4200.

### Stopping it

Press `Ctrl+C` in the API and frontend terminals, then run `docker compose down` from the project root. Your progress stays in a Docker volume until you add `-v`.

## API

| Method | Route                                  | Purpose                                                   |
| ------ | -------------------------------------- | --------------------------------------------------------- |
| GET    | `/api/dashboard/stats`                 | Dashboard counts, overall and per category                |
| GET    | `/api/scenarios`                       | All cards with their progress                             |
| GET    | `/api/scenarios/{id}`                  | One card                                                  |
| GET    | `/api/scenarios/queue`                 | Study queue. Optional `category` and `newLimit` (default 10) |
| POST   | `/api/scenarios/{id}/steps/{stepId}/answer` | Check an answer. A wrong answer applies "Again"      |
| POST   | `/api/scenarios/{id}/review`           | Apply a rating: `Again`, `Hard`, `Good`, `Easy`, or `VeryEasy` |

Answers are checked on the server, and card payloads never include which option is correct.

## Scheduling

| Rating    | New card | Second review | After that             |
| --------- | -------- | ------------- | ---------------------- |
| Again     | < 10 min | < 10 min      | < 10 min               |
| Hard      | 1 hour   | 4 hours       | Interval × 1.2         |
| Good      | 4 hours  | 12 hours      | Interval × ease        |
| Easy      | 12 hours | 1 day         | Interval × ease × 1.3  |
| Very Easy | 1 day    | 2 days        | Interval × ease × 1.7  |

The intervals are deliberately short, because interview prep happens over days rather than weeks.

The first two reviews use fixed intervals, whatever the card's ease. Multipliers only apply once a card has passed both. Again sends a card back to the "New card" column, and it's the only rating that can shorten an interval. If a calculated or fixed value is shorter than the card's current interval, the card keeps its current interval. For example, Very Easy (1 day) followed by Good stays at 1 day rather than dropping to 12 hours.

After the second review, intervals are whole days. Hard always lands at least 1 day past the previous interval, and each rating is at least 1 day longer than the rating before it, so a short interval can't round to the same value on two buttons.

Ease starts at 2.5 and never falls below 1.3. Intervals are capped at 365 days.

| Mastery level | Interval         |
| ------------- | ---------------- |
| Learning      | Under 2 days     |
| Reviewing     | 2 days to 4 days |
| Mastered      | 5 days or more   |

Always pressing Good reaches Mastered on the fourth review, at 4 hours, 12 hours, 2 days, then 5 days.

Intervals run from the moment of review, and a study session only serves cards that are already due. The dashboard's "Cards to Review Today" also counts cards coming due later in the current study day, which ends at midnight in `America/Guatemala` (UTC-6). Timestamps are stored in UTC. Only the end of the study day uses the time zone, and it's calculated in `Services/StudyCalendar.cs`. The next-review time on the dashboard is shown in your browser's time zone.

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
│   │   │   └── SeedData.cs                                # IT support scenario deck
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
│   │   │   ├── StudyCalendar.cs                           # Guatemala study day boundary
│   │   │   └── StudyService.cs                            # Queue, answer checking, progress
│   │   ├── appsettings.json
│   │   ├── Program.cs
│   │   └── SleepyDuck.Api.csproj
│   ├── SleepyDuck.Api.Tests/                              # xUnit tests, described in test.md
│   │   ├── DashboardStatsTests.cs
│   │   ├── SeedDataTests.cs
│   │   ├── SleepyDuck.Api.Tests.csproj
│   │   ├── Sm2SchedulerTests.cs
│   │   └── StudyCalendarTests.cs
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
├── rules.md                                               # Project brief
└── test.md                                                # How the project was tested
```
