using BasicGameService.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace BasicGameService.DTOs
{
    public class DeviceUpdateDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public PlayStationType Type { get; set; }

        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        public bool IsAvailable { get; set; }

        public List<int>? InstalledGameIds { get; set; }
    }
}
