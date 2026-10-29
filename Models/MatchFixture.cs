using System.ComponentModel.DataAnnotations;

namespace CampusLanTournamentManager.Models
{
    public class MatchFixture
    {
        [Key]
        public int MatchId { get; set; }

        [Required]
        [Display(Name = "Tournament Stage")]
        public string RoundStage { get; set; } = "Quarter-Final"; // Quarter-Final, Semi-Final, Final

        [Required]
        [Display(Name = "Team A")]
        public string TeamAName { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Team B")]
        public string TeamBName { get; set; } = string.Empty;

        [Range(0, 50, ErrorMessage = "Score cannot be negative.")]
        public int ScoreTeamA { get; set; } = 0;

        [Range(0, 50, ErrorMessage = "Score cannot be negative.")]
        public int ScoreTeamB { get; set; } = 0;

        [Display(Name = "Winner")]
        public string? WinnerTeam { get; set; }

        public string MatchStatus { get; set; } = "Scheduled"; // Scheduled, Live, Completed

        [Display(Name = "LAN Table / Station #")]
        public string StationNumber { get; set; } = "Zone A - Bay 1";
    }
}