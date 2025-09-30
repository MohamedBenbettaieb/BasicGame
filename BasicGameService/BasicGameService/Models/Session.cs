namespace BasicGameService.Models
{
    public class Session
    {
        public int Id { get; set; }
        public string PlayerName { get; set; } = string.Empty;
        public int DeviceId { get; set; } // FK to Device
        public int? GameId { get; set; } // FK to Game being played, optional
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; } // null if ongoing

        public Device? Device { get; set; }
        public Game? Game { get; set; }
    }
}
