namespace BasicGameService.Models
{
    public static class Database
    {
        public static List<Device> Devices { get; set; } = new()
    {
        new Device { Id = 1, Name = "PS3 #1", Type = PlayStationType.PS3, IsAvailable = true },
        new Device { Id = 2, Name = "PS4 #1", Type = PlayStationType.PS4, IsAvailable = true }
    };

        public static List<Game> Games { get; set; } = new()
    {
        new Game { Id = 1, Name = "FIFA 23", gameType = GameType.football },
        new Game { Id = 2, Name = "Need for Speed", gameType = GameType.racing }
    };

        public static List<Session> Sessions { get; set; } = new();
    }
}
