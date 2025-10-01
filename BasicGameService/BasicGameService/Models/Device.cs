using BasicGameService.Models.Enums;

namespace BasicGameService.Models
{
    public class Device
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty; // Device name
        public PlayStationType Type { get; set; }
        public string Description { get; set; } = string.Empty;
        public bool IsAvailable { get; set; }        
        public Session? CurrentSession { get; set; } // Optional: current session if in use
        public List<Game>? InstalledGames { get; set; } // Optional: list of games associated with the device    

    }
}
