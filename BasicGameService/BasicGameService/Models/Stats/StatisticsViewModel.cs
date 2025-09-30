namespace BasicGameService.Models.Stats
{
    public class StatisticsViewModel
    {
        public List<DeviceStat> DeviceStats { get; set; } = new();
        public List<GameStat> GameStats { get; set; } = new();
    }
}
