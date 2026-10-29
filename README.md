# Campus LAN Gaming & Esports Tournament Manager

An ASP.NET Core MVC web application designed to streamline campus LAN gaming events, team registrations via multi-step forms, BYOD vs. lab hardware logging, and real-time tournament brackets.

## Tech Stack
- **Framework:** ASP.NET Core 8.0 MVC
- **Database:** MySQL via Entity Framework Core (Pomelo)
- **Frontend:** Bootstrap 5 (Custom Dark Esports HUD Theme)
- **State Management:** TempData, Session State

## Implemented Features
1. **Multi-Step Team Registration:** State-managed team info and player roster entry using `TempData`.
2. **Hardware & BYOD Audit Log:** Tracks personal rigs (e.g., ASUS TUF laptops) vs. lab workstation requirements, with MAC verification toggling.
3. **Match Brackets CRUD:** Schedule fixtures, log live scores, and auto-advance winners.
4. **Auto-Database Generation:** Automatically provisions `LanEsportsDb` and tables on startup via `Database.EnsureCreated()`.

## Setup Instructions
1. Ensure MySQL is running on port 3306.
2. Update connection string credentials in `appsettings.json`.
3. Run:
   ```bash
   dotnet restore
   dotnet run

# Poster
<img width="1792" height="2400" alt="Dotnet Poster" src="https://github.com/user-attachments/assets/9ecccaa1-9ec5-4e40-ae9e-8f248804d1c5" />
