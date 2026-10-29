using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CampusLanTournamentManager.Data;
using CampusLanTournamentManager.Models;

namespace CampusLanTournamentManager.Controllers
{
    public class TournamentController : Controller
    {
        private readonly AppDbContext _context;

        public TournamentController(AppDbContext context)
        {
            _context = context;
        }

        // ==========================================
        // DASHBOARD
        // ==========================================
        public async Task<IActionResult> Index()
        {
            ViewBag.TotalTeams = await _context.Teams.CountAsync();
            ViewBag.TotalPlayers = await _context.Players.CountAsync();
            ViewBag.ByodCount = await _context.Players.CountAsync(p => p.HardwareChoice == "BYOD");
            ViewBag.LabRequiredCount = await _context.Players.CountAsync(p => p.HardwareChoice == "LabSystem");

            var recentMatches = await _context.MatchFixtures
                .OrderByDescending(m => m.MatchId)
                .Take(6)
                .ToListAsync();

            return View(recentMatches);
        }

        // ==========================================
        // MULTI-STEP REGISTRATION (Using TempData)
        // ==========================================

        // STEP 1: Team Basics
        [HttpGet]
        public IActionResult RegisterStep1()
        {
            return View(new Team());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RegisterStep1(Team team)
        {
            if (!ModelState.IsValid)
                return View(team);

            // Serialize team basics into TempData
            TempData["TeamBasicInfo"] = JsonSerializer.Serialize(team);
            return RedirectToAction(nameof(RegisterStep2));
        }

        // STEP 2: Players & Hardware Allocation
        [HttpGet]
        public IActionResult RegisterStep2()
        {
            if (TempData["TeamBasicInfo"] == null)
                return RedirectToAction(nameof(RegisterStep1));

            // Retain TempData for the subsequent POST request
            TempData.Keep("TeamBasicInfo");

            // Starter roster list
            var initialPlayers = new List<Player>
            {
                new Player { InGameName = "", FullName = "", HardwareChoice = "BYOD", DeviceDetails = "ASUS TUF A15 / RTX 4050" },
                new Player { InGameName = "", FullName = "", HardwareChoice = "LabSystem", DeviceDetails = "Requires Lab Workstation" }
            };

            return View(initialPlayers);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterStep2(List<Player> players)
        {
            if (TempData["TeamBasicInfo"] is not string teamJson || string.IsNullOrWhiteSpace(teamJson))
            {
                return RedirectToAction(nameof(RegisterStep1));
            }

            var team = JsonSerializer.Deserialize<Team>(teamJson);

            if (team == null || players == null || !players.Any())
            {
                ModelState.AddModelError("", "Session expired or invalid player data.");
                return View(players);
            }

            _context.Teams.Add(team);
            await _context.SaveChangesAsync();

            foreach (var p in players)
            {
                p.TeamId = team.TeamId;
                _context.Players.Add(p);
            }

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"Team '{team.TeamName}' registered successfully!";
            return RedirectToAction(nameof(HardwareLog));
        }

        // ==========================================
        // HARDWARE LOG & VERIFICATION
        // ==========================================
        public async Task<IActionResult> HardwareLog()
        {
            var players = await _context.Players
                .Include(p => p.Team)
                .OrderBy(p => p.HardwareChoice)
                .ToListAsync();

            return View(players);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleMacVerification(int id)
        {
            var player = await _context.Players.FindAsync(id);
            if (player != null)
            {
                player.MacAddressVerified = !player.MacAddressVerified;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(HardwareLog));
        }

        // ==========================================
        // MATCH BRACKETS & SCORE CRUD
        // ==========================================
        public async Task<IActionResult> Brackets()
        {
            var fixtures = await _context.MatchFixtures.ToListAsync();
            return View(fixtures);
        }

        [HttpGet]
        public IActionResult CreateFixture()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateFixture(MatchFixture fixture)
        {
            if (!ModelState.IsValid)
                return View(fixture);

            _context.MatchFixtures.Add(fixture);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Brackets));
        }

        [HttpGet]
        public async Task<IActionResult> UpdateScore(int id)
        {
            var fixture = await _context.MatchFixtures.FindAsync(id);
            if (fixture == null) return NotFound();
            return View(fixture);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateScore(MatchFixture model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var fixture = await _context.MatchFixtures.FindAsync(model.MatchId);
            if (fixture == null) return NotFound();

            fixture.ScoreTeamA = model.ScoreTeamA;
            fixture.ScoreTeamB = model.ScoreTeamB;
            fixture.MatchStatus = model.MatchStatus;

            // Auto-advance winner
            if (model.ScoreTeamA > model.ScoreTeamB)
                fixture.WinnerTeam = fixture.TeamAName;
            else if (model.ScoreTeamB > model.ScoreTeamA)
                fixture.WinnerTeam = fixture.TeamBName;
            else
                fixture.WinnerTeam = "Draw / In Progress";

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Brackets));
        }
    }
}