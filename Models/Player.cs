using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CampusLanTournamentManager.Models
{
    public class Player
    {
        [Key]
        public int PlayerId { get; set; }

        [Required]
        public int TeamId { get; set; }
        [ForeignKey("TeamId")]
        public Team? Team { get; set; }

        [Required(ErrorMessage = "Player in-game name (IGN) is required.")]
        [StringLength(30)]
        [Display(Name = "In-Game Name (IGN)")]
        public string InGameName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Full name is required.")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Hardware allocation status is required.")]
        [Display(Name = "Hardware Source")]
        public string HardwareChoice { get; set; } = "BYOD"; // "BYOD" or "LabSystem"

        [Display(Name = "System Specs / Laptop Model")]
        public string? DeviceDetails { get; set; } // e.g., "ASUS TUF Ryzen 7 / RTX 3050" or "Lab PC Req"

        public bool MacAddressVerified { get; set; } = false;
    }
}