using System.ComponentModel.DataAnnotations;
using BasicGameService.Models;
using BasicGameService.Models.Enums;

namespace BasicGameService.DTOs
{
    public class DeviceCreateDto
    {
        [Required(ErrorMessage = "Device name is required.")]
        [StringLength(100, ErrorMessage = "Device name cannot exceed 100 characters.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Device type is required.")]
        public PlayStationType Type { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string Description { get; set; } = string.Empty;

        // Optional: list of game IDs to associate with this device
        public List<int>? InstalledGameIds { get; set; }
    }
}
