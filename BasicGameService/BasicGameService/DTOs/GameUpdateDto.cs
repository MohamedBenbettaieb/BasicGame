using BasicGameService.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace BasicGameService.DTOs
{
    public class GameUpdateDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public GameType GameType { get; set; }
    }
}
