using System.ComponentModel.DataAnnotations;

namespace CampusLanTournamentManager.Models
{
    public class Team
    {
        [Key]
        public int TeamId { get; set; }

        [Required(ErrorMessage = "Team name is required.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Team name must be between 3 and 50 characters.")]
        [Display(Name = "Team Name")]
        public string TeamName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Select a game category.")]
        [Display(Name = "Game Category")]
        public string GameCategory { get; set; } = string.Empty; // e.g., GTA V, Valorant, BGMI, Tekken

        [Required(ErrorMessage = "Leader contact email is required.")]
        [EmailAddress(ErrorMessage = "Enter a valid email address.")]
        public string LeaderEmail { get; set; } = string.Empty;

        [Required(ErrorMessage = "Leader phone number is required.")]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Enter a valid 10-digit mobile number.")]
        public string ContactNumber { get; set; } = string.Empty;

        public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;

        public ICollection<Player> Players { get; set; } = new List<Player>();
    }
}