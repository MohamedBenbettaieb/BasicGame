namespace BasicGameService.Models
{
    public class Session
    {
        public int Id { get; set; }
        public int DeviceId { get; set; } // FK to Device
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; } // null if ongoing
        public TimeSpan? Duration
        {
            get => EndTime.HasValue ? EndTime - StartTime : null;
        }
        bool IsActive => EndTime == null;
    }

}
