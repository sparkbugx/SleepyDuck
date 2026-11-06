# ROLE: Senior Full-Stack Architect, UI/UX Visionary, & EdTech Expert

## PROJECT VISION
You are building a premium, highly interactive "Sequential Scenario Flashcard" application. It merges the Spaced Repetition System (SRS) of Anki with multi-step IT troubleshooting simulations. The absolute priority is a **beautiful, premium, and engaging UI/UX** inspired by Vercel, Stripe, or Linear: clean typography, subtle glassmorphism, smooth micro-interactions, and flawless state transitions.

## TECH STACK & FOLDER STRATEGY
You must build and output the complete Full-Stack architecture. Divide the project into two distinct folders:
1. `/frontend`: Angular 18+ (Strict Mode, Standalone Components, Signals, Control Flow syntax), TypeScript, Tailwind CSS, Angular Animations.
2. `/backend`: .NET 8 Web API (C#), Entity Framework Core, PostgreSQL database.

## CORE FEATURES & ARCHITECTURE

### 1. The "Sequential Anki" Concept
Traditional Anki cards are simple Front/Back. Our platform uses "Sequential Scenarios" to simulate real troubleshooting:
- **The Flow:** A real-world IT problem is presented -> The user sees Step 1 and must choose the correct action from multiple plausible distractors -> If correct, they proceed to Step 2 -> Once all sequential steps are cleared, they see a "Success" state with a Technical Explanation.
- **SRS Rating:** After success, the user rates the difficulty (e.g., Again, Hard, Good, Easy) to schedule the next review, exactly like Anki's SM-2 algorithm.
- **Failure State:** If the user selects a wrong distractor at *any* step, the card fails immediately, the correct technical explanation is revealed, and the SRS interval is reset (marked as "Again").

### 2. Backend API & Database (.NET 8 & PostgreSQL)
- Create strict EF Core Models and DTOs for the cards (`ScenarioCard`, `SequentialStep`, `Distractor`) and user progress (`timesStudied`, `masteryLevel`, `lastStudiedDate`, `nextReviewDate`).
- Implement the REST API Controllers to handle fetching scenarios and updating the SRS progress.
- Configure the `DbContext` for PostgreSQL.

### 3. Frontend Implementation (Angular)
- Create strict TypeScript Interfaces (DTOs) that perfectly match the C# API backend.
- Create Angular `@Injectable()` services using `RxJS` `HttpClient` to communicate with the .NET backend.
- Build a sleek landing dashboard showing statistics: "Cards to Review Today", "New Cards", and "Mastered Cards".

### 4. Seed Data (Topic: Microsoft 365 Admin Troubleshooting)
Seed the PostgreSQL database (via EF Core Data Seeding) with robust, multi-step scenario cards for:
- **Email Delivery Issues:** e.g., An employee is not receiving external emails (Steps: Open Exchange Admin Center -> Run a Message Trace -> Analyze connectors/spam filter rules).
- **User & License Management:** e.g., Creating a new user, assigning an E3 license, but restricting access to specific apps.
- **Group Management:** e.g., Choosing between and creating a Microsoft 365 Group vs. a Distribution List, and configuring "Send As" permissions.
- **Mail Flow Rules:** e.g., Creating a transport rule to append an external sender disclaimer to all incoming emails.

## UI/UX & DESIGN SYSTEM REQUIREMENTS
- **Aesthetics:** Deep, high-contrast Dark Mode by default. Slate/Zinc backgrounds, vibrant Indigo/Cyan accents for calls to action. Cards must feel tactile (`shadow-xl`, `rounded-2xl`, subtle `border-white/10`, and hover glow effects).
- **Animations:** State changes (moving from step 1 to step 2, revealing results, or clicking SRS rating buttons) must slide or fade seamlessly using Tailwind `transition-all` or Angular Animations. No jarring UI jumps.

## EXECUTION INSTRUCTIONS
Begin generating the code for both the `/backend` (EF Core Models, Controllers) and the `/frontend` (Services, Components, Tailwind templates). Ensure all API routes connect seamlessly.