using System.ComponentModel.DataAnnotations;

namespace BasicGameService.DTOs
{
    public class SessionCreateDto
    {
        [Required]
        public int DeviceId { get; set; }

        public int? GameId { get; set; }

        [Required]
        [StringLength(100)]
        public string PlayerName { get; set; } = string.Empty;
    }
}
