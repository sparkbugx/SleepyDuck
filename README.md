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
| Good   | 1 day    | 6 days        | Interval × ease           |
| Easy   | 4 days   | 6+ days × 1.3 | Interval × ease × 1.3     |

Ease starts at 2.5 and never falls below 1.3. Intervals are capped at 365 days. A card counts as Mastered at an interval of 21 days or more. "Today" is measured in UTC.

## Adding scenarios

Add to `Scenarios` in `backend/SleepyDuck.Api/Data/SeedData.cs`, then create a migration:

```sh
cd backend
dotnet ef migrations add AddMoreScenarios --project SleepyDuck.Api
```
