# ROLE: Senior Full-Stack Architect, UI/UX Visionary, & EdTech Expert

## PROJECT VISION
You are building a premium, highly interactive "Sequential Scenario Flashcard" application. It merges the Spaced Repetition System (SRS) of Anki with multi-step IT troubleshooting simulations, and it's tuned for short-term interview cramming rather than long-term retention. The absolute priority is a **beautiful, premium, and engaging UI/UX** with its own warm identity, the "Warm Dark Mode Oasis": clean typography, smooth micro-interactions, and flawless state transitions.

## TECH STACK & FOLDER STRATEGY
You must build and output the complete Full-Stack architecture. Divide the project into two distinct folders:
1. `/frontend`: Angular 18+ (Strict Mode, Standalone Components, Signals, Control Flow syntax), TypeScript, Tailwind CSS, Angular's built-in `animate.enter` and `animate.leave` animations.
2. `/backend`: .NET 8 Web API (C#), Entity Framework Core, PostgreSQL database.

## CORE FEATURES & ARCHITECTURE

### 1. The "Sequential Anki" Concept
Traditional Anki cards are simple Front/Back. Our platform uses "Sequential Scenarios" to simulate real troubleshooting:
- **The Flow:** A real-world IT problem is presented -> The user sees Step 1 and must choose the correct action from multiple plausible distractors -> If correct, they proceed to Step 2 -> Once all sequential steps are cleared, they see a "Success" state with a Technical Explanation.
- **SRS Rating:** After success, the user rates the difficulty (Again, Hard, Good, Easy, or Very Easy) to schedule the next review, using the cramming schedule below, based on Anki's SM-2 algorithm.
- **Failure State:** If the user selects a wrong distractor at *any* step, the card fails immediately, the correct technical explanation is revealed, and the SRS interval is reset (marked as "Again").

### 2. Cramming Schedule
Intervals are deliberately aggressive, because interview prep happens over days rather than weeks.

| Rating    | New card | Second review | Third review onward    |
| --------- | -------- | ------------- | ---------------------- |
| Again     | < 10 min | < 10 min      | < 10 min               |
| Hard      | 1 hour   | 4 hours       | Interval × 1.2         |
| Good      | 4 hours  | 12 hours      | Interval × ease        |
| Easy      | 12 hours | 1 day         | Interval × ease × 1.3  |
| Very Easy | 1 day    | 2 days        | Interval × ease × 1.7  |

- **Anti-shrink:** A passed review never shortens a card. Only "Again" can shorten or reset an interval.
- **Mastery:** A card is Learning below 2 days, Reviewing from 2 days, and Mastered from 5 days.
- **Time zone:** The study day rolls over at midnight in `America/Guatemala` (UTC-6). Timestamps are stored in UTC, and each interval runs from the exact moment of review.

### 3. Backend API & Database (.NET 8 & PostgreSQL)
- Create strict EF Core Models and DTOs for the cards (`ScenarioCard`, `SequentialStep`, `Distractor`) and user progress (`timesStudied`, `masteryLevel`, `lastStudiedDate`, `nextReviewDate`).
- Implement the REST API Controllers to handle fetching scenarios and updating the SRS progress.
- Configure the `DbContext` for PostgreSQL.

### 4. Frontend Implementation (Angular)
- Create strict TypeScript Interfaces (DTOs) that perfectly match the C# API backend.
- Create Angular `@Injectable()` services using `RxJS` `HttpClient` to communicate with the .NET backend.
- Build a sleek landing dashboard showing statistics: "Cards to Review Today", "New Cards", and "Mastered Cards".

### 5. Seed Data (Topic: IT Support Troubleshooting)
Seed the PostgreSQL database (via EF Core Data Seeding) with robust, multi-step scenario cards. Every category needs at least 2 scenarios. Option length must not give the answer away, so the correct option can't routinely be the longest or the shortest:
- **Email Delivery Issues:** e.g., An employee is not receiving external emails (Steps: Open Exchange Admin Center -> Run a Message Trace -> Analyze connectors/spam filter rules).
- **User & License Management:** e.g., Creating a new user, assigning an E3 license, but restricting access to specific apps.
- **Group Management:** e.g., Choosing between and creating a Microsoft 365 Group vs. a Distribution List, and configuring "Send As" permissions.
- **Mail Flow Rules:** e.g., Creating a transport rule to append an external sender disclaimer to all incoming emails.
- **VOIP Support:** e.g., One-way audio and SIP 408 Request Timeout errors.
- **Network Troubleshooting:** e.g., Packet loss analysis with MTR and ping, and DHCP scope exhaustion.
- **Mac Network Issues:** e.g., The macOS DNS cache and mDNSResponder, and 802.1X Wi-Fi drops.

## UI/UX & DESIGN SYSTEM REQUIREMENTS
- **Aesthetics:** The "Warm Dark Mode Oasis": warm, calm, and high in contrast. Cards must feel tactile, with soft borders and gentle hover feedback.
- **Light and Dark Mode:** Dark Mode is the default. A toggle in the header switches themes, and the choice is saved in `localStorage`. Tailwind uses `darkMode: 'class'`, with the `dark` class on `<html>`.
- **Oasis Palette:** Defined in `frontend/tailwind.config.js` under the `oasis` color namespace:
  - Deep background (dark): `#1E1E2F`
  - Card background (dark): `#2B2B3E`
  - Accent and success: `#4E5B31`
  - Main background (light): `#F0EDE8`
  - Borders (light): `#D1D1D1`
- **Contrast:** Text must stay readable in both themes. `#4E5B31` is too dark to read as text on `#1E1E2F`, so it's used as a fill behind light text, and accent text in dark mode uses a lighter sage tone.
- **Animations:** State changes (moving from step 1 to step 2, revealing results, or clicking SRS rating buttons) must slide or fade seamlessly using Tailwind `transition-all` or Angular Animations. No jarring UI jumps.

## EXECUTION INSTRUCTIONS
Begin generating the code for both the `/backend` (EF Core Models, Controllers) and the `/frontend` (Services, Components, Tailwind templates). Ensure all API routes connect seamlessly.