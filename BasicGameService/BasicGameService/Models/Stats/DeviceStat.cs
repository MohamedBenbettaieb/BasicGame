namespace BasicGameService.Models.Stats
{
    public class DeviceStat
    {
        public int DeviceId { get; set; }
        public string DeviceName { get; set; } = string.Empty;
        public int TimesUsed { get; set; }
        public double TotalTimeMinutes { get; set; }
    }
}
