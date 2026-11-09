# SleepyDuck testing

This file records how SleepyDuck was tested, where the tests are, and what wasn't tested. The results are from the final run on 6 November 2026.

## Summary

| Check                         | Where it ran                     | Result                                       |
| ----------------------------- | -------------------------------- | -------------------------------------------- |
| Automated backend tests       | Windows 11, on .NET 10           | 79 of 79 passed                              |
| Automated backend tests       | Ubuntu 24.04, on .NET 8.0.133    | 79 of 79 passed                              |
| Same tests, clock on Guatemala | Ubuntu 24.04, `TZ=America/Guatemala` | 79 of 79 passed                          |
| Undoing each fix on purpose   | Windows 11                       | All 4 undone fixes were caught               |
| Migration up and down         | Throwaway PostgreSQL database    | Passed                                       |
| Full app, end to end          | Windows 11, Edge, dev database   | All checks passed, no browser console errors |
| README setup on a clean system | Ubuntu 24.04 container          | Passed, with the exceptions listed below     |

## Where the tests are

The automated tests are all backend tests, in `backend/SleepyDuck.Api.Tests/`. The frontend has no automated tests.

```text
backend/
├── SleepyDuck.Api/                  # The code being tested
│   ├── Data/SeedData.cs
│   ├── Services/Sm2Scheduler.cs
│   ├── Services/StudyCalendar.cs
│   └── Services/StudyService.cs
└── SleepyDuck.Api.Tests/
    ├── DashboardStatsTests.cs       # 6 tests: dashboard and queue on a fixed clock
    ├── SeedDataTests.cs             # 12 tests: quality of the scenario deck
    ├── SleepyDuck.Api.Tests.csproj  # xUnit and the EF Core in-memory provider
    ├── Sm2SchedulerTests.cs         # 52 tests: intervals, ease, and mastery
    └── StudyCalendarTests.cs        # 9 tests: the Guatemala study day
```

Run them from the `backend/` folder:

```sh
dotnet test
```

Stop the API first. On Windows, a running API locks the files that the test build needs to replace.

## What the automated tests cover

### `Sm2SchedulerTests.cs` (52 tests)

| Area            | What's checked                                                                                       |
| --------------- | ---------------------------------------------------------------------------------------------------- |
| New cards       | Each rating gets its cramming interval, and comes due exactly one interval later at any time of day  |
| Second review   | Fixed intervals apply, and a longer existing interval is kept                                        |
| Third review on | Multipliers start here, sub-day intervals move to whole days, and each rating beats the one below it |
| Sequences       | Repeated Good and repeated Hard follow the expected intervals                                        |
| Anti-shrink     | A walk through 16,384 rating sequences finds no passed review that shortens a card                   |
| Again           | It resets the interval, returns the card within minutes, and is the only rating that can shorten one |
| Ease            | It never falls below 1.3, and Very Easy raises it more than Easy                                     |
| Limits          | Intervals stop at 365 days                                                                           |
| Mastery         | Learning below 2 days, Reviewing from 2 days, Mastered from 5 days                                   |
| Cramming path   | Always pressing Good reaches Mastered on the fourth review                                           |

### `StudyCalendarTests.cs` (9 tests)

| Area            | What's checked                                                                                  |
| --------------- | ----------------------------------------------------------------------------------------------- |
| Day boundary    | The day ends at Guatemala midnight, tested at 06:00, 17:59, 18:00, 23:59, and 00:00 local time  |
| Value type      | The boundary is a UTC value, which PostgreSQL requires                                          |
| Offset          | Guatemala is 6 hours behind UTC in both January and July                                        |
| Due times       | A 1-day card reviewed at 7:00 PM comes due at 7:00 PM the next day                              |
| Order           | Near midnight, a longer interval still comes due after a shorter one                            |

### `DashboardStatsTests.cs` (6 tests)

These run the real `StudyService` queries on a fixed clock, using the EF Core in-memory provider.

| Local time in Guatemala | Card due                 | Expected result                     |
| ----------------------- | ------------------------ | ----------------------------------- |
| 5:00 PM                 | 8:00 PM the same day     | Counts as today                     |
| 6:30 PM                 | 9:00 AM the next day     | Doesn't count as today              |
| 10:00 PM                | 11:59 PM the same day    | Counts as today                     |
| 10:00 PM                | Midnight                 | Doesn't count as today              |
| 12:01 AM the next day   | 9:00 AM that day         | Counts as today                     |
| 1 minute before due     | At the due time          | Queue holds the card, then serves it |

The first two rows are the cases the old UTC logic got wrong, because UTC midnight is 6:00 PM in Guatemala.

### `SeedDataTests.cs` (12 tests)

| Area           | What's checked                                                                            |
| -------------- | ----------------------------------------------------------------------------------------- |
| Coverage       | Every category has at least 2 scenarios, and every scenario has at least 3 steps          |
| Options        | Every step has 3 distractors, and no step repeats an option                               |
| Length         | In each of the 7 categories, the correct option is longest on at most a third of steps, and shortest on at most a third |
| Ids            | All card, step, option, and distractor ids are unique                                     |

Correct option longest, by category:

| Category                | Before | After |
| ----------------------- | ------ | ----- |
| Email Delivery          | 8 of 8 | 1 of 8 |
| Users & Licenses        | 7 of 8 | 1 of 8 |
| Group Management        | 6 of 8 | 1 of 8 |
| Mail Flow Rules         | 6 of 8 | 2 of 8 |

With 4 options, pure chance would be about 2 of 8. The correct option is shortest on 1 of those 32 steps, so lengthening the distractors didn't create the opposite giveaway. The VOIP, network, and Mac categories already passed, at 4 of 30 steps.

## Do the tests catch the bugs?

A test that can't fail proves nothing, so I undid each fix on purpose, ran the tests, then restored the code.

| Fix undone                       | Result                                                                    |
| -------------------------------- | ------------------------------------------------------------------------- |
| "Today" back on UTC midnight     | 3 tests failed, including both of the 6:00 PM cases                       |
| Mastered back at 21 days         | 2 tests failed                                                            |
| Reviewing back at 6 days         | The build failed with `CS8510`, because that level could never be reached |
| M365 options back to the old text | 4 tests failed, one per M365 category                                    |

The final run was 79 of 79 after everything was restored.

## Migration checks

I tested the new migrations on a throwaway database, which was deleted afterwards. It held 9 studied cards at the old mastery levels.

| Interval (days) | Before     | After upgrade | After rollback |
| --------------- | ---------- | ------------- | -------------- |
| 0, 0.5, 1       | Learning   | Learning      | Learning       |
| 2, 4            | Learning   | Reviewing     | Learning       |
| 5               | Learning   | Mastered      | Learning       |
| 6, 20           | Reviewing  | Mastered      | Reviewing      |
| 21              | Mastered   | Mastered      | Mastered       |

- `RebalanceM365Options` only updates the `Text` of 22 distractors. EF Core printed a data-loss warning when generating it, but the migration has only `UpdateData` calls and no deletes or column changes. The deck stayed at 14 cards, 62 steps, and 186 distractors.
- `CrammingMasteryThresholds` recalculates stored mastery levels, because a card's level otherwise only changes when it's next reviewed.

Both are applied to the development database, which now has 5 migrations.

## End-to-end check

This ran against the real API, Angular dev server, and PostgreSQL database, with Edge driven by Playwright. Progress was cleared afterwards, so the app has 14 new cards.

| Check                                                                   | Result                                         |
| ----------------------------------------------------------------------- | ---------------------------------------------- |
| Migrations and deck size in the database                                | 5 migrations, 14 cards, 62 steps, 186 distractors |
| M365 steps where the correct option is longest, read from the database  | 5 of 32                                        |
| One card due 1 minute before local midnight, one due 1 minute after     | Today count is 1, and neither is due yet       |
| Good four times through the API                                         | Learning, Learning, Reviewing, Mastered        |
| Intervals on that path                                                  | 4 hours, 12 hours, 2 days, 5 days              |
| Mastered tile on the dashboard                                          | Shows 1 of 14                                  |
| M365 study card                                                         | Shows 4 options                                |
| Browser console                                                         | No errors or warnings                          |

## Clean Ubuntu check

I followed the README's Ubuntu steps in a new Ubuntu 24.04 container, on a real .NET 8 install, with an empty PostgreSQL database.

| Step                                 | Result                                       |
| ------------------------------------ | -------------------------------------------- |
| Package install                      | .NET SDK 8.0.133, ASP.NET Core 8.0.33, Node 22.23.4, npm 10.9.9 |
| Backend tests                        | 79 of 79 passed                              |
| Backend tests with the clock on Guatemala | 79 of 79 passed                         |
| API on an empty database             | 5 migrations applied, 14 cards               |
| `npm ci` and `npm start`             | Page returned 200                            |
| API through the frontend proxy       | Returned 14 cards                            |

## What wasn't tested

- **Frontend logic:** there are no automated frontend tests. Interval formatting and the study screens were checked in a browser in earlier rounds. This round's browser check covered only the dashboard and the first step of one M365 card, since no frontend code changed.
- **Long options at phone width:** the longer M365 options weren't viewed at 400px.
- **Ubuntu service setup:** `docker compose up`, `systemctl`, and `usermod` can't run in that container. PostgreSQL ran in a separate container, and I only confirmed that the compose file parses.
- **Windows installs:** the `winget install` commands weren't run. I only confirmed that the package ids exist.
- **Migrations on Windows from empty:** the empty-database run was on Ubuntu. On Windows they were applied to the existing database.
- **Technical accuracy:** the scenarios were written from memory, not checked against current vendor documentation. Automated tests check the structure of the deck, not whether the content is correct. I read each of the 22 rewritten M365 options against its explanation, and that review was by hand.
- **Daylight saving changes:** Guatemala doesn't currently use daylight saving time. If that changed, the calendar would follow the system's time zone data, but that case wasn't tested.

## Where the one-off checks are

The migration, undo-a-fix, end-to-end, and Ubuntu checks were temporary scripts, and they aren't part of this repository. Only the tests in `backend/SleepyDuck.Api.Tests/` can be rerun from the project.


## First test completed